using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    Rigidbody2D rigid;
    public GameObject explosion;
    float timer = 0.2f;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector2(PlayerScript.instance.dir * 6, 11);
    }

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (timer > 0) return;
        Destroy(Instantiate(explosion, transform.position, Quaternion.identity),0.8f);  
        Destroy(gameObject);
    }
}
