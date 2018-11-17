using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public SkinnedMeshRenderer MonsterMirror;
    public ParticleSystem Particles;

    public Coroutine xRayCoroutine;

    public void StartXRayVision(float duration)
    {
        if (xRayCoroutine != null)
        {
            StopCoroutine(xRayCoroutine);
        }
        xRayCoroutine = StartCoroutine(XRayVisionCoroutine(duration));
    }

    private IEnumerator XRayVisionCoroutine(float duration)
    {
        MonsterMirror.enabled = true;
        if (!Particles.isPlaying)
        {
            Particles.Play();
        }

        yield return new WaitForSeconds(duration);
        xRayCoroutine = null;
        MonsterMirror.enabled = false;
    }
}
