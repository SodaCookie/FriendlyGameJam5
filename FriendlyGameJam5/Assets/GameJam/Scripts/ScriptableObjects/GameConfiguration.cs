using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[Serializable]
public struct GamePhase
{
    public GamePhase(float gameTime, int additionalMonsters, float newMonsterWalkSpeed, float newMonsterAggroSpeed)
    {
        this.gameTime = gameTime;
        this.additionalMonsters = additionalMonsters;
        this.newMonsterWalkSpeed = newMonsterWalkSpeed;
        this.newMonsterAggroSpeed = newMonsterAggroSpeed;
    }

    public float gameTime;
    public int additionalMonsters;
    public float newMonsterWalkSpeed;
    public float newMonsterAggroSpeed;
}

[CreateAssetMenu(fileName = "Game Configuration")]
public class GameConfiguration : ScriptableObject
{
    [Header("Pre-Game Attributes")]
    public bool playIntro = true;
    public float startFiresPerMinute = 1f;
    public float finalFiresPerMinute = 3f;
    public float stationHealth = 100f;
    [Range(0, float.PositiveInfinity)] public float gameDuration = 10f;

    [Header("In-Game Attributes")]
    public List<GamePhase> gamePhases = new List<GamePhase>(new GamePhase[] { new GamePhase(0, 1, 0.2f, 0.6f), new GamePhase(7, 1, 0.3f, 0.7f) });
    public float defaultMonsterWalkSpeed = 0.2f;
    public float defaultMonsterAggroSpeed = 0.6f;

    [Header("Player Attributes")]
    public float playerSprintDuration = 4f;
    public float playerHealingTime = 5f;
}
