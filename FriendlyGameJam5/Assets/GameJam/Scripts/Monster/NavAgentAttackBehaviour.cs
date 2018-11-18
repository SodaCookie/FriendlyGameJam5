using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NavAgentPathingBehaviour), typeof(Animator))]
public class NavAgentAttackBehaviour : MonoBehaviour {

    public float stunDuration = 3;

    private NavAgentPathingBehaviour pathingBehaviour;
    private bool attacked = false;
    private Animator animator;

    private void Awake()
    {
        pathingBehaviour = GetComponent<NavAgentPathingBehaviour>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !attacked)
        {
            animator.SetBool(Animator.StringToHash("Attack"), true);
            pathingBehaviour.PausePathing();
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
        pathingBehaviour.ResumePathing();
        attacked = false;
    }
}
