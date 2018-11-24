using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    public SkinnedMeshRenderer Mirror;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GetComponent<UnityEngine.AI.NavMeshAgent>().speed = GameManager.Instance.gameConfiguration.monsterWalkSpeed;
            GetComponent<NavAgentSightBehaviour>().defaultSpeed = GameManager.Instance.gameConfiguration.monsterWalkSpeed;
            GetComponent<NavAgentSightBehaviour>().aggroSpeed = GameManager.Instance.gameConfiguration.monsterAggroSpeed;
            if (GameManager.Instance.IsCrunchTime)
            {
                GetComponent<UnityEngine.AI.NavMeshAgent>().speed += GameManager.Instance.gameConfiguration.crunchTimeSpeed;
                GetComponent<NavAgentSightBehaviour>().defaultSpeed += GameManager.Instance.gameConfiguration.crunchTimeSpeed;
                GetComponent<NavAgentSightBehaviour>().aggroSpeed += GameManager.Instance.gameConfiguration.crunchTimeSpeed;
            }
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
