using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour {

    public GameObject ClosedCollider;
    public string ClosedStateName = "door_1_closed";
    public string OpeningStateName = "door_1_open";
    public string ClosingStateName = "door_1_close";
    public AudioSource DoorOpenSource;
    public AudioSource DoorCloseSource;
    private Animator animator;

    private int entitiesNearby = 0;

    private bool openSoundPlayed = false;
    private bool closeSoundPlayed = true;

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

        if (current.shortNameHash == Animator.StringToHash(OpeningStateName) && !openSoundPlayed)
        {
            DoorOpenSource.Play();
            DoorCloseSource.Stop();
            openSoundPlayed = true;
            closeSoundPlayed = false;
        }

        if (current.shortNameHash == Animator.StringToHash(ClosingStateName) && !closeSoundPlayed)
        {
            DoorOpenSource.Stop();
            DoorCloseSource.Play();
            openSoundPlayed = false;
            closeSoundPlayed = true;
        }
	}
}
