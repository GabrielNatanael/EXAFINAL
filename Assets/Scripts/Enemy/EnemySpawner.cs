using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] Transform player;
    [SerializeField] Transform spawnPoint;
    [SerializeField] KeyCode spawnKey = KeyCode.E;

    private void Update()
    {
        if(Input.GetKeyDown(spawnKey))
        {
           SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if(enemyPrefab != null && spawnPoint != null)
        {
            GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            Enemy enemyScript = enemy.GetComponent<Enemy>();

            if(enemyScript != null&& player != null)
            {
                enemyScript.SetPlayer(player);
            }
            else
            {
                Debug.LogWarning("Enemy script or player transform has not been assigned");
            }
        }
        else
        {
            Debug.LogWarning("Enemy prefab or spawnpoint not assigned");
        }
    }
}
