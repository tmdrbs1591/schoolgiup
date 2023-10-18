using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    int enemiesToSpawn;
    EnemyBase spawnedEnemy;
    public GameObject[] enemyToSpawn;

    private void Start()
    {
        enemiesToSpawn = Mathf.Min(3,1 + (int)Mathf.Floor(GameManager.instance.doorsBroken / 3));
    }

    void Update()
    {
        if (enemiesToSpawn > 0 && (spawnedEnemy == null || spawnedEnemy.dead))
            Spawn();
    }

    void Spawn()
    {
        int rand = Random.Range(0, enemyToSpawn.Length);
        spawnedEnemy = Instantiate(enemyToSpawn[rand], transform.position, Quaternion.identity).GetComponent<EnemyBase>();
        enemiesToSpawn--;
        if (enemiesToSpawn <= 0) Destroy(gameObject);
    }
}
