using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ThirdPersonCharacter))]
public class NavAgentPathingBehaviour : MonoBehaviour
{
    public Transform target;
    public NavigationPath path;

    private NavMeshAgent agent;
    private NavAgentSightBehaviour sightBehaviour;
    private ThirdPersonCharacter character;
    private Transform curWaypoint;
    private bool pathing = true;
    private bool checkingLocation = false;
    private Coroutine wayPointWait;

    private void Start()
    {
        curWaypoint = path.GetNextWaypoint();
        agent = GetComponent<NavMeshAgent>();
        sightBehaviour = GetComponent<NavAgentSightBehaviour>();
        character = GetComponent<ThirdPersonCharacter>();
        if (agent == null) Debug.LogError("NavMeshAgent component missing from GameObject", gameObject);
        agent.updateRotation = false;
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

    IEnumerator WaitAtWaypoint(float duration)
    {
        yield return new WaitForSeconds(duration);
        curWaypoint = path.GetNextWaypoint();
        wayPointWait = null;
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
                        checkingLocation = true;
                    }
                    else
                    {
                        NavMeshHit hit;
                        if (checkingLocation)
                        {
                            if (NavMesh.SamplePosition(sightBehaviour.lastSeen, out hit, 1, NavMesh.AllAreas))
                            {
                                agent.SetDestination(hit.position);
                            }
                        }
                        else
                        {
                            if (NavMesh.SamplePosition(target.position, out hit, 1, NavMesh.AllAreas))
                            {
                                agent.SetDestination(hit.position);
                            }
                        }


                        if (agent.remainingDistance < agent.stoppingDistance)
                        {
                            checkingLocation = false;
                        }
                    }
                }
                else
                {
                    agent.SetDestination(curWaypoint.position);
                    if (agent.remainingDistance < agent.stoppingDistance && wayPointWait == null)
                    {
                        wayPointWait = StartCoroutine(WaitAtWaypoint(path.StopTime));
                    }
                }

                if (agent.remainingDistance < agent.stoppingDistance)
                {
                    character.Move(Vector3.zero, false, false);
                    Debug.Log("Stopping");
                }
                else
                {
                    Debug.Log("Moving");
                    character.Move(agent.desiredVelocity, false, false);
                }
            }
        }
    }
}
