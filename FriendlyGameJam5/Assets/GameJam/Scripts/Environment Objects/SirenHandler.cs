using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SirenHandler : MonoBehaviour
{
    public float Speed = 1;
    public int EmissionMaterialIndex = 1;
    public Color StrobColor = Color.red;
    public Light LightReference;

    private new MeshRenderer renderer;
    private Material material;
    private Color defaultColor;
    private Color lightDefaultColor;
    private bool strob = true;

    // Use this for initialization
    void Start()
    {
        // Duplicate the material on start so no mass changes
        renderer = GetComponent<MeshRenderer>();
        var mats = renderer.materials;
        renderer.materials = mats;
        material = renderer.materials[EmissionMaterialIndex];
        defaultColor = renderer.materials[EmissionMaterialIndex].GetColor("_EmissionColor");

        if (LightReference)
        {
            lightDefaultColor = LightReference.color;
        }
    }

    public void StartStrob()
    {
        strob = true;
    }

    public void StopStrob()
    {
        strob = false;
    }

    private void Update()
    {
        float t = 0;
        if (strob)
        {
            t = Mathf.Sqrt(Mathf.Abs(((Time.time * Speed) % 2) - 1));
        }
        material.SetColor("_EmissionColor", Color.Lerp(defaultColor, StrobColor, t));
        if (LightReference)
        {
            LightReference.color = Color.Lerp(lightDefaultColor, StrobColor, t);
        }
    }
}
