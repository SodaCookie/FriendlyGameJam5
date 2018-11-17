using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStationSprinkler : MonoBehaviour {
    public ParticleSystem sprinklerParticles;

    public void TurnOn()
    {
        sprinklerParticles.Play();
    }

    public void TurnOff()
    {
        sprinklerParticles.Stop();
    }
}
