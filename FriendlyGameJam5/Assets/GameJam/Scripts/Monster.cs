using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    public SkinnedMeshRenderer Mirror;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().speed = GameManager.Instance.CurrentMonsterWalkSpeed;
            GetComponent<NavAgentSightBehaviour>().defaultSpeed = GameManager.Instance.CurrentMonsterWalkSpeed;
            GetComponent<NavAgentSightBehaviour>().aggroSpeed = GameManager.Instance.CurrentMonsterAggroSpeed;
            GameManager.Instance.Monsters.Add(this);
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Monsters.Remove(this);
        }
    }
}
