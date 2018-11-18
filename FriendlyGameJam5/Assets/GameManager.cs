using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;

        originalFogRedValue = RenderSettings.fogColor.r;
    }

    [HideInInspector] public Player ThePlayer;
    [HideInInspector] public Monster TheMonster;
    [HideInInspector] public List<FireStation> FireStations = new List<FireStation>();

    public float MinutesUntilVictory;
    public float StartFiresPerMinute;
    public float FinalFiresPerMinute;
    public float GraceMinutesBeforeFailure;

    public AnimationCurve WarningLightRedPulse;

    private List<FireStation> safeFireStations = new List<FireStation>();
    private List<FireStation> onFireStations = new List<FireStation>();

    private bool emergencyMode = false; // When failure is iminent
    private float originalFogRedValue;

    private void OnEnable()
    {
        StartCoroutine(FireStationGameloop());
    }

    public void MarkFireStationAsSafe(FireStation station)
    {
        emergencyMode = false;
        onFireStations.Remove(station);
        safeFireStations.Add(station);
    }

    public void MarkFireStationAsOnFire(FireStation station)
    {
        onFireStations.Add(station);
        safeFireStations.Remove(station);
    }

    IEnumerator FireStationGameloop()
    {
        yield return null;
        float startTime = Time.time;
        float goalTime = startTime + (60 * MinutesUntilVictory);
        float lastFire = float.MinValue;
        while(Time.time < goalTime)
        {
            float progress = (Time.time - startTime) / (60 * MinutesUntilVictory);
            float frequency = Mathf.Lerp(StartFiresPerMinute, FinalFiresPerMinute, progress) / 60;
            float period = 1 / frequency;
            if (Time.time > lastFire + period)
            {
                FireStation station = GetRandomSafeFirestation();
                if (station != null)
                {
                    station.SetOnFire();
                    lastFire = Time.time;
                    Debug.Log(station + " was just set on fire!");
                }
            }

            if (safeFireStations.Count == 0)
            {
                emergencyMode = true;
                StartCoroutine(FailureIminent());
                while (emergencyMode) yield return null;
            }
            else
            {
                yield return null;
            }
        }
        GameWin();
    }

    IEnumerator FailureIminent()
    {
        float startTime = Time.time;
        float failTime = startTime + (60 * GraceMinutesBeforeFailure);
        StartCoroutine(RedWarningLights());
        yield return new WaitUntil(() => { return Time.time >= failTime || !emergencyMode; });
        if (!emergencyMode) {
            yield break;
        }
        GameOver();
    }

    IEnumerator RedWarningLights()
    {
        float startTime = Time.time;
        while (emergencyMode)
        {
            float elapsedTime = Time.time - startTime;
            Color newCol = RenderSettings.fogColor;
            newCol.r = WarningLightRedPulse.Evaluate(elapsedTime);
            RenderSettings.fogColor = newCol;
            yield return null;
        }
    }

    private void GameOver()
    {
        Time.timeScale = 0;
        Debug.Log("You lost!");
    }

    private void GameWin()
    {
        Time.timeScale = 0;
        Debug.Log("You won!");
    }

    private FireStation GetRandomSafeFirestation()
    {
        if (safeFireStations.Count <= 0) return null;
        int index = Mathf.RoundToInt(Random.Range(0, safeFireStations.Count - 1));
        return safeFireStations[index];
    }
}
