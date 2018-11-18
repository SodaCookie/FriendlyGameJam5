using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioController : MonoBehaviour {
    public AudioSource SprintBreathing;
    public AudioSource DamagedBreathing;
    public AudioSource HealingBreathing;

    public SimpleCharacterControl controller;
    public Player player;

    private int startingHealth;

    private void Awake()
    {
        startingHealth = player.Health;
    }

    private void Update()
    {
        if (player.Health != startingHealth && !DamagedBreathing.isPlaying)
        {
            DamagedBreathing.Play();
            HealingBreathing.Stop();
            SprintBreathing.Stop();
        }
        else if (player.Health == startingHealth && DamagedBreathing.isPlaying && !HealingBreathing.isPlaying)
        {
            HealingBreathing.Play();
            DamagedBreathing.Stop();
            SprintBreathing.Stop();
        }
        else if (!DamagedBreathing.isPlaying && ! HealingBreathing.isPlaying && !controller.CanSprint)
        {
            Debug.Log("Sound");
            SprintBreathing.Play();
        }
    }
}
