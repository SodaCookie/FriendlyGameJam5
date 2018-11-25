﻿using System.Collections;
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
    [HideInInspector] public List<Monster> Monsters = new List<Monster>();
    [HideInInspector] public List<FireStation> FireStations = new List<FireStation>();


    [Header("Monster Spawning Parameters")]
    public float MonsterMinDistance = 30f;
    public AudioSource SpawnSound;
    public GameObject MonsterPrefab;

    [Header("References")]
    public FireStation StartingFireStation;
    public NavigationPath MonsterWaypoints;
    public AudioSource IntroAnnouncement;
    public AudioSource WarningAnnouncement;
    public AudioSource EvacuationAnnouncement;
    public AnimationCurve WarningLightRedPulse;

    [HideInInspector] public bool IsGameOver = false;
    [HideInInspector] public bool IsVictory = false;
    [HideInInspector] public GameConfiguration gameConfiguration;

    private List<FireStation> safeFireStations = new List<FireStation>();
    private List<FireStation> onFireStations = new List<FireStation>();

    private bool emergencyMode = false; // When failure is iminent
    private bool monsterSpawned = false;
    private bool monsterCanSpawn = false;

    private float originalFogRedValue;
    private bool introMode = true;

    // Game State
    public bool IsCrunchTime { get; private set; }
    public float GameTime { get; private set; }
    public float StationHealth { get; private set; }

    private void OnEnable()
    {
        introMode = true;
        IsCrunchTime = false;
        gameConfiguration = GameModeLoader.Instance.gameConfiguration;
        StationHealth = gameConfiguration.stationHealth;
        GameTime = 0;
        StartCoroutine(FireStationGameloop());
        StartCoroutine(PlayerHealthGameloop());
    }

    public void SpawnMonster()
    {
        print(1);
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
        monster.GetComponent<NavAgentSightBehaviour>().target = ThePlayer.RaycastPoint;
        ThePlayer.GetComponent<Player>().MonsterMirrors.Add(monster.GetComponent<Monster>().Mirror);
    }

    IEnumerator MonsterSpawnSoundDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        SpawnSound.Play();
    }

    IEnumerator EvacuationAnnouncementSoundDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        EvacuationAnnouncement.Play();
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
            monsterSpawned = true;
        }
    }

    public void MarkFireStationAsOnFire(FireStation station)
    {
        onFireStations.Add(station);
        safeFireStations.Remove(station);

        monsterCanSpawn = true;
    }

    IEnumerator MonsterSpawnGameloop()
    {
        float startTime = Time.time;
        float goalTime = startTime + (60 * gameConfiguration.gameDuration);
        List<float> releaseTimes = new List<float>(gameConfiguration.monsterReleaseTimes);
        while (Time.time - startTime < goalTime && releaseTimes.Count > 0)
        {
            foreach (float spawnTime in gameConfiguration.monsterReleaseTimes)
            {
                if (spawnTime < Time.time - startTime && releaseTimes.Contains(spawnTime))
                {
                    releaseTimes.Remove(spawnTime);
                    SpawnMonster();
                }
            }
            yield return null;
        }
    }

    IEnumerator MonsterCrunchWait()
    {
        float startTime = Time.time;
        yield return new WaitForSeconds(gameConfiguration.crunchTime * 60);
        IsCrunchTime = true;
        if (Monsters.Count > 0)
        {
            foreach (var monster in Monsters)
            {
                monster.GetComponent<UnityEngine.AI.NavMeshAgent>().speed += gameConfiguration.crunchTimeSpeed;
                monster.GetComponent<NavAgentSightBehaviour>().defaultSpeed += gameConfiguration.crunchTimeSpeed;
                monster.GetComponent<NavAgentSightBehaviour>().aggroSpeed += gameConfiguration.crunchTimeSpeed;
            }
        }
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
        if (gameConfiguration.playIntro)
        {
            introMode = true;
            StartingFireStation.SetOnFire();
            IntroAnnouncement.Play();
            yield return new WaitUntil(() => { return !introMode; });
        }

        // Start announcer for time remaining
        if (gameConfiguration.gameDuration > 3)
        {
            StartCoroutine(EvacuationAnnouncementSoundDelay((gameConfiguration.gameDuration - 3) * 60));
        }
        StartCoroutine(MonsterCrunchWait());
        StartCoroutine(MonsterSpawnGameloop());

        float startTime = Time.time;
        float goalTime = startTime + (60 * gameConfiguration.gameDuration);
        float lastFire = float.MinValue;
        while (Time.time < goalTime)
        {
            GameTime = Time.time - startTime;
            StationHealth -= 0.06f * Time.deltaTime * onFireStations.Count;
            float progress = (Time.time - startTime) / (60 * gameConfiguration.gameDuration);
            float frequency = Mathf.Lerp(gameConfiguration.startFiresPerMinute, gameConfiguration.finalFiresPerMinute, progress) / 60;
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

            if (StationHealth / gameConfiguration.stationHealth < 0.1)
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
        float failTime = startTime + (60 * gameConfiguration.graceMinutesBeforeFailure);
        StartCoroutine(RedWarningLights());
        yield return new WaitUntil(() => { return StationHealth <= 0 || !emergencyMode; });
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
        Cursor.visible = true;
        IsGameOver = true;
        Debug.Log("You lost!");
    }

    private void GameWin()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
