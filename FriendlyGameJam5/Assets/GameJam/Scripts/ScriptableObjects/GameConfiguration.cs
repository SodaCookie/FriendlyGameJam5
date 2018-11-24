using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[CreateAssetMenu(fileName = "Game Configuration")]
public class GameConfiguration : ScriptableObject
{
    [Header("General")]
    public bool playIntro = true;
    public float startFiresPerMinute = 1f;
    public float finalFiresPerMinute = 3f;
    public float crunchTime = 7f;
    public float crunchTimeSpeed = 0.1f;
    public float graceMinutesBeforeFailure = 0.5f;
    public float stationHealth = 100f;
    [Range(0, float.PositiveInfinity)] public float gameDuration = 10f;

    [Header("Monster Attributes")]
    public List<float> monsterReleaseTimes = new List<float>(new float[] { 0f, 7f });
    public float monsterWalkSpeed = 0.2f;
    public float monsterAggroSpeed = 0.6f;

    [Header("Player Attributes")]
    public float playerSprintDuration = 4f;
    public float playerHealingTime = 5f;
}
