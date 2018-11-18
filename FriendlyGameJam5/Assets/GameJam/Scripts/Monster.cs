using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TheMonster = this;
        }
    }
}
