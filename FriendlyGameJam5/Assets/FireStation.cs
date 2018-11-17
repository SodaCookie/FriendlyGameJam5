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
        SetOnFire();
    }

    public void SetOnFire()
    {
        OnFire = true;
        EnableWhenOnFire.SetActive(true);
        ControllingButton.SetToWarningMode();
        Fire.Play();
    }

    public void PutOutFire()
    {
        OnFire = false;
        EnableWhenOnFire.SetActive(false);
        ControllingButton.SetToNormalMode();
        Fire.Stop();
    }
}
