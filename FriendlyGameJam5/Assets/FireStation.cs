using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStation : MonoBehaviour {
    public GameObject EnableWhenOnFire;
    public FireStationButton ControllingButton;
    public ParticleSystem Fire;

    public bool OnFire { get; private set; }

    public void SetOnFire()
    {
        OnFire = true;
        EnableWhenOnFire.SetActive(true);
        RecursiveSetLayer(transform, LayerMask.NameToLayer("Fire"));
        ControllingButton.SetToWarningMode();
        Fire.Play();
    }

    public void PutOutFire()
    {
        OnFire = false;
        EnableWhenOnFire.SetActive(false);
        ControllingButton.SetToNormalMode();
        RecursiveSetLayer(transform, LayerMask.NameToLayer("Default"));
        Fire.Stop();
    }

    private void RecursiveSetLayer(Transform trans, int layer)
    {
        trans.gameObject.layer = layer;
        foreach (Transform child in trans) {
            RecursiveSetLayer(child, layer);
        }
    }
}
