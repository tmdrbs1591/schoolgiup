using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    Rigidbody2D rigid;
    public GameObject explosion;
    public GameObject explosionsmoke;
    float timer = 0.2f;
    [SerializeField] Transform explosionPos;

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
        Destroy(Instantiate(explosion, explosionPos.transform.position, Quaternion.identity),0.8f);
        Destroy(Instantiate(explosionsmoke, explosionPos.transform.position, Quaternion.identity), 1.7f);
        Destroy(gameObject);
    }


    IEnumerator Smoke() { 
        yield return new WaitForSeconds(0.6f);
      
    }
}
