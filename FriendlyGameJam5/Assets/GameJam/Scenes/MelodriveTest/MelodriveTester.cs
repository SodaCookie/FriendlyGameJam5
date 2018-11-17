using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelodriveTester : MonoBehaviour {

    Melodrive.MelodrivePlugin driver;

	// Use this for initialization
	void Update () {
        driver = GetComponent<Melodrive.MelodrivePlugin>();
        driver.SetEmotion(Melodrive.Emotions.Emotion.Tense);
        driver.SetStyle(Melodrive.Styles.Style.Rock);
        driver.SetEnsemble(Melodrive.Ensembles.Ensemble.EpicRock);
        driver.SetTempoScale(144);
	}
}
