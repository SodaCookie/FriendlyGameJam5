using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickerLight : MonoBehaviour {

    public Light light;
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
            if (Random.value > 0.95)
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
