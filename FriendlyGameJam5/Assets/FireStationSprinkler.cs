using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStationSprinkler : MonoBehaviour {
    public ParticleSystem sprinklerParticles;
    public GameObject EnableWhenSprinkler;

    public void TurnOn()
    {
        sprinklerParticles.Play();
        EnableWhenSprinkler.SetActive(true);
    }

    public void TurnOff()
    {
        EnableWhenSprinkler.SetActive(false);
        sprinklerParticles.Stop();
    }
}
