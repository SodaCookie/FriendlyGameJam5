using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterParticle : MonoBehaviour {

    public ParticleSystem Particles;
    public GameObject ToDestroy;

    private void Update()
    {
        if (!Particles.IsAlive())
        {
            Destroy(ToDestroy);
        }
    }
}
