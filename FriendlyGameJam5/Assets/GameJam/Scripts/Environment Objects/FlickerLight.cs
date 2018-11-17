using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour {

    public Light light;
    [Range(0, 1)] public float flickerChance = 0.05f;
    public float lifetime = 30;

    private float defaultIntensity;

    private void Awake()
    {
        defaultIntensity = light.intensity;
        StartCoroutine(BeginFlickerLight());
    }

    private IEnumerator BeginFlickerLight()
    {
        float startTime = Time.time;

        while (Time.time - startTime < lifetime)
        {
            if (Random.value > 1 - flickerChance)
            {
                light.intensity = 0;
            }
            else
            {
                light.intensity = defaultIntensity;
            }
            yield return null;
        }
        light.intensity = 0;
    }
}
