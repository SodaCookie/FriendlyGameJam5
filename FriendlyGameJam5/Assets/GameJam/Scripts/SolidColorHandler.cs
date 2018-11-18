using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolidColorHandler : MonoBehaviour {

    private UnityEngine.UI.Image image;
    public float duration = 1f;
    public ApplicationHandler app;

    private void Awake()
    {
        image = GetComponent<UnityEngine.UI.Image>();
        StartCoroutine(FadeIn());
    }

    public void RestartGame()
    {
        StartCoroutine(FadeOutRetry());
    }

    public void OpenMainMenu()
    {
        StartCoroutine(FadeOutMenu());
    }

    IEnumerator FadeIn()
    {
        image.enabled = true;
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            image.color = Color.Lerp(Color.black, Color.clear, (Time.time - startTime) / duration);
            yield return null;
        }
        image.enabled = false;
    }

    IEnumerator FadeOutMenu()
    {
        image.enabled = true;
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            image.color = Color.Lerp(Color.clear, Color.black, (Time.time - startTime) / duration);
            yield return null;
        }
        app.OpenMainMenu();
    }

    IEnumerator FadeOutRetry()
    {
        image.enabled = true;
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            image.color = Color.Lerp(Color.clear, Color.black, (Time.time - startTime) / duration);
            yield return null;
        }
        app.StartGame();
    }
}
