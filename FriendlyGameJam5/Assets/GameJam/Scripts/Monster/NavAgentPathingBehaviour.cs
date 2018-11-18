using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentPathingBehaviour : MonoBehaviour
{
    public Transform target;
    public NavigationPath path;

    private NavMeshAgent agent;
    private NavAgentSightBehaviour sightBehaviour;
    private Transform curWaypoint;
    private bool pathing = true;

    private void Awake()
    {
        curWaypoint = path.GetNextWaypoint();
        agent = GetComponent<NavMeshAgent>();
        sightBehaviour = GetComponent<NavAgentSightBehaviour>();
        if (agent == null) Debug.LogError("NavMeshAgent component missing from GameObject", gameObject);
    }

    private void OnEnable()
    {
        StartCoroutine(FollowCoroutine());
    }

    public void PausePathing()
    {
        agent.SetDestination(transform.position);
        pathing = false;
    }

    public void ResumePathing()
    {
        pathing = true;
    }

    IEnumerator FollowCoroutine()
    {
        while (true)
        {
            yield return null;
            if (pathing)
            {
                if (sightBehaviour.aggro)
                {
                    if (sightBehaviour.targetVisible)
                    {
                        NavMeshHit hit;
                        if (NavMesh.SamplePosition(target.position, out hit, 1, NavMesh.AllAreas))
                        {
                            agent.SetDestination(hit.position);
                        }
                    }
                    else
                    {
                        NavMeshHit hit;
                        if (NavMesh.SamplePosition(sightBehaviour.lastSeen, out hit, 1, NavMesh.AllAreas))
                        {
                            agent.SetDestination(hit.position);
                        }
                    }

                }
                else
                {
                    if ((agent.transform.position - curWaypoint.position).magnitude < 1f)
                    {
                        curWaypoint = path.GetNextWaypoint();
                    }
                    agent.SetDestination(curWaypoint.position);
                }
            }
        }
    }
}
