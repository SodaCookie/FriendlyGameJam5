using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeLoader : MonoBehaviour {

    public static GameModeLoader Instance { get; private set; }

    public GameConfiguration gameConfiguration;

    private void Awake()
    {
        if (Instance != null) Destroy(this);
        else Instance = this;

        DontDestroyOnLoad(this.gameObject);
    }
}
