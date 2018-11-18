using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSplash : MonoBehaviour
{

    int CurrentHealth = 2;
    UnityEngine.UI.Image image;
    public UnityEngine.UI.Image solidColor;
    public GameObject GameOver;
    private bool dead = false;
    Color defaultColor;

    private void Awake()
    {
        image = GetComponent<UnityEngine.UI.Image>();
        defaultColor = image.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.ThePlayer.Health != CurrentHealth)
        {
            if (CurrentHealth > GameManager.Instance.ThePlayer.Health && !dead)
            {
                StartCoroutine(AnimateRedSplash());
            }

            CurrentHealth = GameManager.Instance.ThePlayer.Health;

            if (CurrentHealth == 2)
            {
                StartCoroutine(AnimateHealingDamage());
            }
            else if (CurrentHealth == 1)
            {
                StartCoroutine(AnimateWarningDamage());
            }
        }

        if (GameManager.Instance.IsGameOver && !dead)
        {
            dead = true;
            GameOver.SetActive(true);
            StartCoroutine(AnimateDeath());
        }
    }

    IEnumerator AnimateWarningDamage()
    {
        image.enabled = true;
        image.color = defaultColor;

        while (CurrentHealth < 2)
        {
            float t = Mathf.Pow(Mathf.Abs((Time.time * 2 % 2) - 1), 2.0f);
            Color tmpColor = image.color;
            tmpColor.a = Mathf.Lerp(defaultColor.a, 1, t);
            image.color = tmpColor;
            float scale = Mathf.Lerp(1, 1.3f, t);
            image.transform.localScale = new Vector3(scale, scale, scale);
            yield return null;
        }
    }

    IEnumerator AnimateRedSplash()
    {
        solidColor.enabled = true;
        float startTime = Time.time;
        while (Time.time - startTime < 0.4f)
        {
            Color tmpColor = Color.red;
            tmpColor.r = 0.9f;
            tmpColor.a = Mathf.Sqrt(1 - ((Time.time - startTime) / 0.4f)) * 0.8f;
            solidColor.color = tmpColor;
            yield return null;
        }
        solidColor.enabled = false;
    }

    IEnumerator AnimateHealingDamage()
    {
        Color prevColor = image.color;
        float startTime = Time.time;
        while (Time.time - startTime < 1.5f)
        {
            Color tmpColor = Color.red;
            image.color = Color.Lerp(prevColor, Color.clear, Mathf.Sqrt((Time.time - startTime) / 1.5f));
            yield return null;
        }
        image.enabled = false;
    }

    IEnumerator AnimateDeath()
    {
        yield return new WaitForSeconds(0.5f);
        solidColor.enabled = true;
        float startTime = Time.time;
        while (Time.time - startTime < 1.5f)
        {
            Color tmpColor = Color.black;
            tmpColor.a = Mathf.Sqrt((Time.time - startTime) / 1.5f);
            solidColor.color = tmpColor;
            yield return null;
        }
    }
}
