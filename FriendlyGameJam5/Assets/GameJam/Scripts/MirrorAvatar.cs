using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorAnimator : MonoBehaviour {

    public Animator animator;
    public new SkinnedMeshRenderer renderer;

    public void DisplayXRay()
    {
        renderer.enabled = true;
    }

    public void HideXRay()
    {
        renderer.enabled = true;
    }

    void LateUpdate()
    {
        GetComponent<Animator>().SetFloat("Forward", animator.GetFloat("Forward"));
        GetComponent<Animator>().SetFloat("Turn", animator.GetFloat("Turn"));
        GetComponent<Animator>().SetBool("Crouch", animator.GetBool("Crouch"));
        GetComponent<Animator>().SetBool("OnGround", animator.GetBool("OnGround"));
        GetComponent<Animator>().SetFloat("Jump", animator.GetFloat("Jump"));
        GetComponent<Animator>().SetFloat("JumpLeg", animator.GetFloat("JumpLeg"));
    }
}
