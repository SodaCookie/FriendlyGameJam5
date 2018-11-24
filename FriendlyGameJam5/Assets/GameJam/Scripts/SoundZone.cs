using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundZone : MonoBehaviour {
    public AudioListener targetListener;
    public List<AudioSource> sources = new List<AudioSource>();

    private int insideTriggerCount = 0;

    private void Start()
    {
        foreach (AudioSource source in sources)
        {
            source.mute = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == targetListener.gameObject)
        {
            insideTriggerCount++;
            if (insideTriggerCount == 1)
            {
                foreach (AudioSource source in sources)
                {
                    source.mute = false;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == targetListener.gameObject)
        {
            insideTriggerCount--;
            if (insideTriggerCount <= 0)
            {
                foreach (AudioSource source in sources)
                {
                    source.mute = true;
                }
            }
        }
    }
}
