using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<SkinnedMeshRenderer> MonsterMirrors = new List<SkinnedMeshRenderer>();
    public Transform RaycastPoint;
    public ParticleSystem Particles;

    public Coroutine xRayCoroutine;
    public int Health = 2;
    
    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ThePlayer = this;
            GetComponent<SimpleCharacterControl>().m_sprintMeterMax = GameManager.Instance.gameConfiguration.playerSprintDuration;
            GetComponent<SimpleCharacterControl>().m_sprintMeter = GameManager.Instance.gameConfiguration.playerSprintDuration;
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
        yield return new WaitForSeconds(GameManager.Instance.gameConfiguration.playerHealingTime);
        Health += 1;
    }

    private IEnumerator XRayVisionCoroutine(float duration)
    {
        foreach (var mirror in MonsterMirrors)
        {
            mirror.enabled = true;
        }
        if (!Particles.isPlaying)
        {
            Particles.Play();
        }

        yield return new WaitForSeconds(duration);
        xRayCoroutine = null;
        foreach (var mirror in MonsterMirrors)
        {
            mirror.enabled = false;
        }
    }
}
