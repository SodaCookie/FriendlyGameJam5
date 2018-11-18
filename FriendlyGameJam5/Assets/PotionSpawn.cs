using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSpawn : MonoBehaviour {
    public GameObject PotionPrefab;
    [Range(0, 1)]
    public float chanceOfSpawn;

    private GameObject lastSpawned;

    public void Roll()
    {
        bool rollSuccess = Random.value <= chanceOfSpawn;
        if (rollSuccess && lastSpawned == null)
        {
            lastSpawned = Instantiate(PotionPrefab, transform.position, Quaternion.identity);
        }
    }
}
