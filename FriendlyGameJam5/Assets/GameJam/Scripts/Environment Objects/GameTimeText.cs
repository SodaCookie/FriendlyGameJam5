using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeText : MonoBehaviour {

    private TMPro.TextMeshProUGUI text;

	// Use this for initialization
	void Awake () {
        text = GetComponent<TMPro.TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
        int minutes = Mathf.FloorToInt(GameManager.Instance.gameConfiguration.gameDuration * 60 - GameManager.Instance.GameTime) / 60;
        int seconds = Mathf.FloorToInt((GameManager.Instance.gameConfiguration.gameDuration * 60 - GameManager.Instance.GameTime) % 60);
        text.text = string.Format("{0}:{1}", minutes.ToString("D2"), seconds.ToString("D2"));
	}
}
