using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    public float MonsterMinDistance = 30f;
    public AudioSource SpawnSound;
    public GameObject MonsterPrefab;
    public FireStation StartingFireStation;
    public NavigationPath MonsterWaypoints;
    public AudioSource IntroAnnouncement;
    public AudioSource WarningAnnouncement;
    public AudioSource EvacuationAnnouncement;

    public float MinutesUntilVictory;
    public float CrunchTimeMinute;
    public float StartFiresPerMinute;
    public float FinalFiresPerMinute;
    public float GraceMinutesBeforeFailure;

    public AnimationCurve WarningLightRedPulse;

    [HideInInspector] public bool IsGameOver = false;
    [HideInInspector] public bool IsVictory = false;

    private List<FireStation> safeFireStations = new List<FireStation>();
    private List<FireStation> onFireStations = new List<FireStation>();

    private bool emergencyMode = false; // When failure is iminent
    private bool monsterSpawned = false;
    private bool monsterCanSpawn = false;
    private float originalFogRedValue;
    private bool introMode = true;

    private void OnEnable()
    {
        introMode = true;
        StartCoroutine(FireStationGameloop());
        StartCoroutine(PlayerHealthGameloop());
    }

    public void SpawnMonster()
    {
        // Find valid location
        Vector3 location = MonsterWaypoints.GetNextWaypoint().position;
        StartCoroutine(MonsterSpawnSoundDelay(2f));
        while ((ThePlayer.transform.position - location).magnitude < MonsterMinDistance)
        {
            location = MonsterWaypoints.GetNextWaypoint().position;
        }
        GameObject monster = Instantiate(MonsterPrefab, location, Quaternion.identity);
        monster.GetComponent<NavAgentPathingBehaviour>().target = ThePlayer.transform;
        monster.GetComponent<NavAgentPathingBehaviour>().path = MonsterWaypoints;
        monster.GetComponent<NavAgentSightBehaviour>().target = ThePlayer.transform;
        ThePlayer.GetComponent<Player>().MonsterMirrors.Add(monster.GetComponent<Monster>().Mirror);
    }

    IEnumerator MonsterSpawnSoundDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        SpawnSound.Play();
    }

    public void MarkFireStationAsSafe(FireStation station)
    {
        onFireStations.Remove(station);
        safeFireStations.Add(station);

        if (safeFireStations.Count > 1)
        {
            emergencyMode = false;
        }

        if (introMode)
        {
            introMode = false;
        }

        if (!monsterSpawned && monsterCanSpawn)
        {
            SpawnMonster();
            monsterSpawned = true;
        }
    }

    public void MarkFireStationAsOnFire(FireStation station)
    {
        onFireStations.Add(station);
        safeFireStations.Remove(station);

        monsterCanSpawn = true;
    }

    IEnumerator MonsterCrunchWait()
    {
        float startTime = Time.time;
        yield return new WaitForSeconds(CrunchTimeMinute * 60);
        EvacuationAnnouncement.Play();
        if (TheMonster != null)
        {
            TheMonster.GetComponent<UnityEngine.AI.NavMeshAgent>().speed = 2;
            TheMonster.GetComponent<NavAgentSightBehaviour>().defaultSpeed = 2;
            TheMonster.GetComponent<NavAgentSightBehaviour>().aggroSpeed = 6;
        }
        SpawnMonster();
    }

    IEnumerator PlayerHealthGameloop()
    {
        yield return null;
        while (ThePlayer.Health > 0)
        {
            yield return null;
        }
        GameOver();
    }

    IEnumerator FireStationGameloop()
    {
        yield return null;
        introMode = true;
        StartingFireStation.SetOnFire();
        IntroAnnouncement.Play();
        yield return new WaitUntil(() => { return !introMode; });
        StartCoroutine(MonsterCrunchWait());
        float startTime = Time.time;
        float goalTime = startTime + (60 * MinutesUntilVictory);
        float lastFire = float.MinValue;
        while (Time.time < goalTime)
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
                }
            }

            if (safeFireStations.Count <= 1)
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
        WarningAnnouncement.Play();
        float startTime = Time.time;
        float failTime = startTime + (60 * GraceMinutesBeforeFailure);
        StartCoroutine(RedWarningLights());
        yield return new WaitUntil(() => { return Time.time >= failTime || !emergencyMode; });
        if (!emergencyMode)
        {
            Color newCol = RenderSettings.fogColor;
            newCol.r = originalFogRedValue;
            RenderSettings.fogColor = newCol;
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
        Cursor.lockState = CursorLockMode.None;
        IsGameOver = true;
        Debug.Log("You lost!");
    }

    private void GameWin()
    {
        Cursor.lockState = CursorLockMode.None;
        IsVictory = true;
        var allAudioSources = Object.FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (var audioS in allAudioSources)
        {
            audioS.Stop();
        }
        Debug.Log("You won!");
    }

    private FireStation GetRandomSafeFirestation()
    {
        if (safeFireStations.Count <= 0) return null;
        int index = Mathf.RoundToInt(Random.Range(0, safeFireStations.Count - 1));
        return safeFireStations[index];
    }
}
