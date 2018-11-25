using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameMode : MonoBehaviour {

    public GameConfiguration gameConfiguration;
    public TransitionHandler transitions;

	public void StartGame()
    {
        transitions.StartGame(gameConfiguration);
    }
}
