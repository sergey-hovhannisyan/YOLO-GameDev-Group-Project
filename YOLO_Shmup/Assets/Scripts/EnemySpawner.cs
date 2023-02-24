using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float instantiateTime = 1f;
    public int numberOfEnemies = 20;
    public GameObject enemyPrefab;
    public GameObject blackHolePrefab;


    IEnumerator Start()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector2 spawnPos = new Vector2(Random.Range(-8.25f, 8.25f), Random.Range(-4.5f, 4.5f));
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            Instantiate(blackHolePrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(instantiateTime);
        }
    }
}
