using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentFollowBehaviour : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent agent;
    private bool following = true;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) Debug.LogError("NavMeshAgent component missing from GameObject", gameObject);
    }

    public void StartFollowing()
    {
        following = true;
    }

    public void StopFollowing()
    {
        following = false;
    }

    private void OnEnable()
    {
        StartCoroutine(FollowCoroutine());
    }

    IEnumerator FollowCoroutine()
    {
        following = true;
        while (true)
        {
            yield return null;
            if (following)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(target.position, out hit, 1, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                }
            }
            else
            {
                agent.SetDestination(transform.position);
            }
        }
    }
}
