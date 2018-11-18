using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NavAgentFollowBehaviour), typeof(Animator))]
public class NavAgentAttackBehaviour : MonoBehaviour {

    public float stunDuration = 3;

    private NavAgentFollowBehaviour followBehaviour;
    private bool attacked = false;
    private Animator animator;

    private void Awake()
    {
        followBehaviour = GetComponent<NavAgentFollowBehaviour>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !attacked)
        {
            animator.SetBool(Animator.StringToHash("Attack"), true);
            followBehaviour.StopFollowing();
            other.GetComponent<Player>().DamagePlayer();
            StartCoroutine(DisableFollow());
            attacked = true;
        }
    }

    private IEnumerator DisableFollow()
    {
        yield return null;
        animator.SetBool(Animator.StringToHash("Attack"), false);
        yield return new WaitForSeconds(stunDuration);
        followBehaviour.StartFollowing();
        attacked = false;
    }
}
