using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionPotionHandler : MonoBehaviour {

    public SpriteRenderer renderer;
    public Light PointLight;
    public float Duration = 10;
    public float FadeTime = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().StartXRayVision(Duration);
            StartCoroutine(FadeAndDestroy());
        }
    }

    private IEnumerator FadeAndDestroy()
    {
        float startTime = Time.time;
        float startIntensity = PointLight.intensity;
        while (Time.time - startTime < FadeTime)
        {
            PointLight.intensity = Mathf.Lerp(startIntensity, 0, (Time.time - startTime) / FadeTime);
            yield return null;
        }
        Destroy(gameObject);
    }
}
