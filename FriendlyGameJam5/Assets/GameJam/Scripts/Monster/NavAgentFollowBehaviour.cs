using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentFollowBehaviour : MonoBehaviour {
    public Transform target;
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) Debug.LogError("NavMeshAgent component missing from GameObject", gameObject);
    }

    private void OnEnable()
    {
        StartCoroutine(FollowCoroutine());
    }

    IEnumerator FollowCoroutine()
    {
        while (true)
        {
            yield return null;
            agent.SetDestination(target.position);
        }
    }
}
