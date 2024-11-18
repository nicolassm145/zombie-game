using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spwanerEnemy : MonoBehaviour
{
    [SerializeField] float spawnRate = 3f;

    [SerializeField] private GameObject[] enemyPrefab;

    [SerializeField] private bool canSpawn = true;
    
    void Start()
    {
        StartCoroutine(Spawner());
    }

    private IEnumerator Spawner()
    {
        WaitForSeconds wait = new WaitForSeconds(spawnRate);

        while (true)
        {
            yield return wait;
            Instantiate(enemyPrefab[Random.Range(0, enemyPrefab.Length)], transform.position, Quaternion.identity);
        }
    }
}
