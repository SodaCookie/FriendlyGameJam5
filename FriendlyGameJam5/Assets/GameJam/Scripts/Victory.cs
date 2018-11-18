using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour {

    public float duration;
    CanvasGroup group;

    private void OnEnable()
    {
        group = GetComponent<CanvasGroup>();
        StartCoroutine(FadeGroupIn());
    }

    IEnumerator FadeGroupIn()
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            group.alpha = (Time.time - startTime) / duration;
            yield return null;
        }
    }
}
