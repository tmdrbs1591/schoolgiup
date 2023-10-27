using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    float explosionTimer = 0.15f;

    void Start()
    {
        AudioScript.instance.PlaySound(transform.position, 13, Random.Range(0.9f, 1.1f), 1);
    }

    // Update is called once per frame
    void Update()
    {
        explosionTimer -= Time.deltaTime;
        if (explosionTimer <= 0)
        {
            Destroy(GetComponent<Collider2D>());
            explosionTimer = 100;
        }
    }
}
