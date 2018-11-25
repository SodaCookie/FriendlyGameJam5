using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class FireStationButton : MonoBehaviour {
    public enum Mode { Normal, Warning }

    public FireStation ControlledFireStation;
    public FireStationSprinkler ControlledSprinkler;

    public GameObject EnableOnWarningMode;
    public GameObject EnableOnNormalMode;

    public Mode CurrentMode { get; private set; }

    private bool alreadyRunningSprinklers = false;

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (CrossPlatformInputManager.GetButtonDown("Interact"))
            {
                ActivateSprinklers();
            }
        }
    }

    public void ActivateSprinklers()
    {
        if (!alreadyRunningSprinklers)
            StartCoroutine(SprinklersCoroutine());
    }

    public void SetToWarningMode()
    {
        EnableOnWarningMode.SetActive(true);
        EnableOnNormalMode.SetActive(false);
        CurrentMode = Mode.Warning;
    }

    public void SetToNormalMode()
    {
        EnableOnWarningMode.SetActive(false);
        EnableOnNormalMode.SetActive(true);
        CurrentMode = Mode.Normal;
    }

    IEnumerator SprinklersCoroutine()
    {
        alreadyRunningSprinklers = true;
        ControlledSprinkler.TurnOn();
        yield return new WaitForSeconds(2);
        ControlledFireStation.PutOutFire();
        yield return new WaitForSeconds(1);
        ControlledSprinkler.TurnOff();
        alreadyRunningSprinklers = false;
    }
}
