using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionHandler : MonoBehaviour {

    public CanvasGroup group;
    public CanvasGroup secondGroup;
    public UnityEngine.UI.Image mask;
    public float duration = 3f;
    public float duration2 = 3f;
    public float duration3 = 3f;
    public float duration4 = 3f;
    private bool done = false;

    public ApplicationHandler app;

    private void Awake()
    {
        StartCoroutine(FadeOutMask());
    }

    private void Update()
    {
        if (!done)
        {
            if (Input.anyKey)
            {
                StopAllCoroutines();
                group.alpha = 1;
                secondGroup.alpha = 2;
                mask.gameObject.SetActive(false);
                done = true;
            }
        }
    }

    public void StartGame()
    {
        StartCoroutine(FadeInMask());
    }

    IEnumerator FadeInMask()
    {
        mask.gameObject.SetActive(true);
        float startTime = Time.time;
        while (Time.time - startTime < duration4)
        {
            mask.color = Color.Lerp(Color.clear, Color.black, Mathf.Sqrt((Time.time - startTime) / duration4));
            yield return null;
        }
        app.StartGame();
    }

    IEnumerator FadeOutMask()
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            mask.color = Color.Lerp(Color.black, Color.clear, Mathf.Sqrt((Time.time - startTime) / duration));
            yield return null;
        }
        mask.gameObject.SetActive(false);
        StartCoroutine(FadeInGroup());
    }

    IEnumerator FadeInGroup()
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration2)
        {
            group.alpha = Mathf.Sqrt((Time.time - startTime) / duration2);
            yield return null;
        }
        StartCoroutine(FadeInSecondGroup());
    }

    IEnumerator FadeInSecondGroup()
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration3)
        {
            secondGroup.alpha = Mathf.Sqrt((Time.time - startTime) / duration3);
            yield return null;
        }
        done = true;
    }
}
