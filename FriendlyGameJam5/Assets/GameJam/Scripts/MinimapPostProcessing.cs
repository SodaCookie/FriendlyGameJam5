using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapPostProcessing : MonoBehaviour {

    public Material material;
    public Camera FireCamera;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetTexture("_MainTexture", source);
        material.SetTexture("_FireTexture", FireCamera.targetTexture);
        RenderTexture background = new RenderTexture(source.width, source.height, source.depth);
        Graphics.Blit(source, destination, material);
    }
}
