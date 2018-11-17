using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour {

    public GameObject ClosedCollider;
    public string ClosedStateName = "door_1_closed";
    private Animator animator;

    private int entitiesNearby = 0;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Monster")
        {
            entitiesNearby++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Monster")
        {
            entitiesNearby--;
        }
    }

    // Update is called once per frame
    void Update () {
        animator.SetBool("character_nearby", entitiesNearby > 0);

        AnimatorStateInfo current = animator.GetCurrentAnimatorStateInfo(0);

        if (current.shortNameHash == Animator.StringToHash(ClosedStateName))
        {
            ClosedCollider.SetActive(true);
        }
        else
        {
            ClosedCollider.SetActive(false);
        }
	}
}
