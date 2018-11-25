using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionHandler : MonoBehaviour {

    public CanvasGroup group;
    public CanvasGroup secondGroup;
    public CanvasGroup modeSelectGroup;
    public UnityEngine.UI.Image mask;
    public float duration = 3f;
    public float duration2 = 3f;
    public float duration3 = 3f;
    public float duration4 = 3f;
    public float modeSelectFadeDuration = 0.5f;
    private bool done = false;

    public ApplicationHandler app;

    private void Awake()
    {
        StartCoroutine(FadeInMask());
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

    public void StartGame(GameConfiguration gc)
    {
        StartCoroutine(FadeOutMaskToGame(gc));
    }

    public void DisplayModeSelect()
    {
        StartCoroutine(FadeOutGroup(group, modeSelectFadeDuration));
        StartCoroutine(FadeOutGroup(secondGroup, modeSelectFadeDuration, FadeInGroup(modeSelectGroup, modeSelectFadeDuration)));
    }
    
    public void HideModeSelect()
    {
        StartCoroutine(FadeOutGroup(modeSelectGroup, modeSelectFadeDuration, new List<IEnumerator>(
            new IEnumerator[] { FadeInGroup(group, modeSelectFadeDuration), FadeInGroup(secondGroup, modeSelectFadeDuration)})));
    }


    IEnumerator FadeOutMaskToGame(GameConfiguration gc)
    {
        mask.gameObject.SetActive(true);
        float startTime = Time.time;
        while (Time.time - startTime < duration4)
        {
            mask.color = Color.Lerp(Color.clear, Color.black, Mathf.Sqrt((Time.time - startTime) / duration4));
            yield return null;
        }
        app.StartGame(gc);
    }

    IEnumerator FadeInMask()
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            mask.color = Color.Lerp(Color.black, Color.clear, Mathf.Sqrt((Time.time - startTime) / duration));
            yield return null;
        }
        mask.gameObject.SetActive(false);
        StartCoroutine(FadeInGroup(group, duration2, FadeInGroup(secondGroup, duration3)));
    }

    IEnumerator FadeInGroup(CanvasGroup group, float duration, IEnumerator chain = null)
    {
        group.gameObject.SetActive(true);
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            group.alpha = Mathf.Sqrt((Time.time - startTime) / duration);
            yield return null;
        }
        group.alpha = 1;

        if (chain != null)
        {
            StartCoroutine(chain);
        }
    }

    IEnumerator FadeOutGroup(CanvasGroup group, float duration, IEnumerator chain = null)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            group.alpha = 1 - Mathf.Sqrt((Time.time - startTime) / duration);
            yield return null;
        }
        group.alpha = 0;
        group.gameObject.SetActive(false);

        if (chain != null)
        {
            StartCoroutine(chain);
        }
    }

    IEnumerator FadeOutGroup(CanvasGroup group, float duration, List<IEnumerator> chains)
    {
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            group.alpha = 1 - Mathf.Sqrt((Time.time - startTime) / duration);
            yield return null;
        }
        group.alpha = 0;
        group.gameObject.SetActive(false);

        if (chains != null)
        {
            foreach (IEnumerator chain in chains)
            {
                StartCoroutine(chain);
            }
        }
    }
}
