using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbientSoundManager : MonoBehaviour {
    public float MinSoundPeriod;
    public float MaxSoundPeriod;
    public List<AudioClip> sounds;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) Debug.LogError("No AudioSource component on AmbientSoundManager", gameObject);
    }

    private void OnEnable()
    {
        StartCoroutine(AmbientSounds());
    }

    IEnumerator AmbientSounds()
    {
        while (true)
        {
            float period = Random.Range(MinSoundPeriod, MaxSoundPeriod);
            yield return new WaitForSeconds(period);
            int soundIndex = Mathf.RoundToInt(Random.Range(0, sounds.Count - 1));
            audioSource.PlayOneShot(sounds[soundIndex]);
            yield return new WaitUntil(() => { return !audioSource.isPlaying; });
        }
    }
}
