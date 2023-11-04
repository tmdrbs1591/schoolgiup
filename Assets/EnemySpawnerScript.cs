using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    int enemiesToSpawn;
    [SerializeField] int spawnAfter;
    [SerializeField] int spawnFor;
    EnemyBase spawnedEnemy;
    public GameObject[] enemyToSpawn;

    private void Start()
    {
        enemiesToSpawn = Mathf.Min(3,1 + (int)Mathf.Floor((GameManager.instance.doorsBroken - spawnAfter) / 3));
        if (enemiesToSpawn <= 0 || GameManager.instance.doorsBroken > spawnFor) Destroy(gameObject);
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
