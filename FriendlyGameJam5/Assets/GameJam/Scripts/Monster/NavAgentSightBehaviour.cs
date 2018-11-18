using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentSightBehaviour : MonoBehaviour {

    public float sightRange = 20;
    public float aggroSpeed = 5;
    public float deaggroCooldownDuration = 3;
    public Transform target;
    public LayerMask oculusionMask;
    public Transform headBone;

    private NavMeshAgent agent;
    private float defaultSpeed;
    public bool aggro;
    public bool targetVisible;
    public Vector3 lastSeen;
    private Coroutine cooldownCoroutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        defaultSpeed = agent.speed;
    }

    void LateUpdate () {
        targetVisible = HasLineOfSight();
        if (targetVisible)
        {
            UpdateLookAtBehaviour();
        }
        else
        {
            UpdateLookAwayBehaviour();
        }
    }

    private bool HasLineOfSight()
    {
        if (target)
        {
            Vector3 origin = transform.position;
            if (headBone)
            {
                origin = headBone.position;
            }
            RaycastHit raycastHit;
            Physics.Raycast(origin, target.position - origin, out raycastHit, sightRange, oculusionMask);

            if (raycastHit.collider != null && raycastHit.collider.gameObject == target.gameObject)
            {
                lastSeen = raycastHit.point;
            }

            return raycastHit.collider != null && raycastHit.collider.gameObject == target.gameObject;
        }
        return false;
    }

    private void UpdateLookAtBehaviour()
    {
        // Check if just started aggro
        if (!aggro)
        {
            aggro = true;
            agent.speed = aggroSpeed;
        }

        // Handle movement speed
        if (cooldownCoroutine != null)
        {
            StopCoroutine(cooldownCoroutine);
        }

        // Handle head turning
        if (target && headBone)
        {
            headBone.rotation = Quaternion.Slerp(headBone.rotation, Quaternion.LookRotation(target.position - headBone.position), 0.5f);
        }
    }

    private void UpdateLookAwayBehaviour()
    {
        if (aggro && cooldownCoroutine == null)
        {
            StartCoroutine(StartCooldown(deaggroCooldownDuration));
        }
    }

    private IEnumerator StartCooldown(float duration)
    {
        yield return new WaitForSeconds(duration);
        aggro = false;
        agent.speed = defaultSpeed;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
