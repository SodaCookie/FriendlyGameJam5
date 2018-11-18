using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public SkinnedMeshRenderer MonsterMirror;
    public ParticleSystem Particles;

    public Coroutine xRayCoroutine;
    public float HealingTime = 5;
    public int Health = 2;
    
    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ThePlayer = this;
        }
    }

    public void DamagePlayer()
    {
        Health -= 1;
        GetComponent<SimpleCharacterControl>().m_sprintMeter = GetComponent<SimpleCharacterControl>().m_sprintMeterMax;
        GetComponent<SimpleCharacterControl>().m_recharging = false;

        StartCoroutine(BeginHealing());
    }

    public void StartXRayVision(float duration)
    {
        if (xRayCoroutine != null)
        {
            StopCoroutine(xRayCoroutine);
        }
        xRayCoroutine = StartCoroutine(XRayVisionCoroutine(duration));
    }

    private IEnumerator BeginHealing()
    {
        yield return new WaitForSeconds(HealingTime);
        Health += 1;
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
