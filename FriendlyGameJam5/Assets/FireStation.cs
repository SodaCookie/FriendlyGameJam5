using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStation : MonoBehaviour {
    public GameObject EnableWhenOnFire;
    public FireStationButton ControllingButton;
    public ParticleSystem Fire;

    public bool OnFire { get; private set; }

    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.FireStations.Add(this);
            GameManager.Instance.MarkFireStationAsSafe(this);
        }
    }

    public void SetOnFire()
    {
        if (OnFire) return;
        OnFire = true;
        EnableWhenOnFire.SetActive(true);
        RecursiveSetLayer(transform, LayerMask.NameToLayer("Fire"));
        ControllingButton.SetToWarningMode();
        Fire.Play();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.MarkFireStationAsOnFire(this);
        }
    }

    public void PutOutFire()
    {
        if (!OnFire) return;
        OnFire = false;
        EnableWhenOnFire.SetActive(false);
        ControllingButton.SetToNormalMode();
        RecursiveSetLayer(transform, LayerMask.NameToLayer("Default"));
        Fire.Stop();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.MarkFireStationAsSafe(this);
        }
    }

    private void RecursiveSetLayer(Transform trans, int layer)
    {
        trans.gameObject.layer = layer;
        foreach (Transform child in trans) {
            RecursiveSetLayer(child, layer);
        }
    }
}
