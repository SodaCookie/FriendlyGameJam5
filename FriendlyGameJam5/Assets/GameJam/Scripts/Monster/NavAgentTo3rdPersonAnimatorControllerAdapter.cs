using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class NavAgentTo3rdPersonAnimatorControllerAdapter : MonoBehaviour {
    public float MaxAnimationVelocity; // What velocity should we set our animation to 1
    public float MaxAnimationTurnSpeed;
    private NavMeshAgent agent;
    private Animator animator;

    private Rigidbody rbody;
    private Vector3 lastFacing;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null) Debug.LogError("NavMeshAgent component missing from GameObject", gameObject);
        animator = GetComponent<Animator>();
        if (agent == null) Debug.LogError("Animator component missing from GameObject", gameObject);

        rbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        animator.SetFloat("Forward", agent.velocity.magnitude / MaxAnimationVelocity);

        // Handle rotation
        Vector3 currentFacing = transform.forward;
        float currentAngularVelocity = Vector3.SignedAngle(currentFacing, lastFacing, Vector3.down) / Time.deltaTime; //degrees per second
        Debug.Log(currentAngularVelocity);
        animator.SetFloat("Turn", currentAngularVelocity / MaxAnimationTurnSpeed);
        lastFacing = currentFacing;
    }
}
