using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FootstepsAudio : MonoBehaviour {
    public AudioSource leftFootSource;
    public AudioSource rightFootSource;
    public Collider leftFootStepTrigger;
    public Collider rightFootStepTrigger;

    public List<AudioClip> footsteps;

    private void OnTriggerEnter(Collider other)
    {
        if (other == leftFootStepTrigger)
        {
            int index = Mathf.RoundToInt(Random.Range(0, footsteps.Count - 1));
            leftFootSource.PlayOneShot(footsteps[index]);
        }
        if (other == rightFootStepTrigger)
        {
            int index = Mathf.RoundToInt(Random.Range(0, footsteps.Count - 1));
            rightFootSource.PlayOneShot(footsteps[index]);
        }
    }
}
