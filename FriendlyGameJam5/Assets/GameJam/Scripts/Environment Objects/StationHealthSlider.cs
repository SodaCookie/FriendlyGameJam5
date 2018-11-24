using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationHealthSlider : MonoBehaviour {

    UnityEngine.UI.Slider slider;

	// Use this for initialization
	void Awake () {
        slider = GetComponent<UnityEngine.UI.Slider>();
	}
	
	// Update is called once per frame
	void Update () {
        slider.value = 1 - GameManager.Instance.StationHealth / GameManager.Instance.gameConfiguration.stationHealth;
	}
}
