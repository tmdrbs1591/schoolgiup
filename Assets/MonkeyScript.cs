using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyScript : MonoBehaviour
{
    [SerializeField] Transform ball;
    Rigidbody2D rb;
    SpriteRenderer spriterend;
    bool attacking;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriterend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ball == null)
        {
            rb.velocity = new Vector2(rb.velocity.x / 1.2f, rb.velocity.y);
            spriterend.flipX = PlayerScript.instance.transform.position.x > transform.position.x;
        }
        else
        {
            spriterend.flipX = ball.position.x > transform.position.x;
            if (attacking)
            {
                if (transform.localPosition.y < -0.6f)
                {
                    if (Mathf.Abs(transform.position.x) - Mathf.Abs(ball.position.x) < 3)
                    {
                        rb.velocity += Vector2.up * 14;
                        attacking = false;
                    }
                } else
                {
                    if (Mathf.Abs(transform.position.x) - Mathf.Abs(ball.position.x) > 3)
                        rb.velocity = new Vector2((transform.position.x < ball.position.x ? 10 : -10), rb.velocity.y);
                }
            }
            else
            {
                if (transform.localPosition.y < -0.6f)
                {
                    if (transform.localPosition.x < 23)
                    {
                        rb.velocity = new Vector2(4, 7);
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, 7);
                    }
                    if (Mathf.Abs(transform.position.x) - Mathf.Abs(ball.position.x) < 3)
                    {
                        rb.velocity += Vector2.up * 14;
                        attacking = true;
                    }
                }
                else
                {
                    rb.velocity = new Vector2(-2, rb.velocity.y);
                }
            }
        }
    }
}
