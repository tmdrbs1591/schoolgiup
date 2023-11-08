using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballScript : MonoBehaviour
{
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "WeaponHurtBox")
        {
            rb.velocity = new Vector2(PlayerScript.instance.dir * 15, 20);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioScript.instance.PlaySound(transform.position, 16, Random.Range(0.8f, 1.0f), rb.velocity.magnitude / 10);
        if (collision.gameObject.tag == "Monke")
        {
            rb.velocity = new Vector2(-15, 20);
        }
    }
}
