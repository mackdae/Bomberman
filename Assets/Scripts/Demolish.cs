using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demolish : MonoBehaviour
{
    public float demolishTime = 1f;

    [Range(0f, 1f)] public float spawnChance = 0.2f;
    public GameObject[] spawnItems;

    void Start()
    {
        Destroy(gameObject, demolishTime);
    }

    private void OnDestroy()
    {
        if (spawnItems.Length > 0 && Random.value < spawnChance)
        {
            int randomIndex = Random.Range(0, spawnItems.Length);
            Instantiate(spawnItems[randomIndex], transform.position, Quaternion.identity);
        }
    }
}