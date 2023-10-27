using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    Rigidbody2D rigid;
    [SerializeField] PhysicsMaterial2D material;
    int bounces;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = new Vector2(PlayerScript.instance.dir * 50, 6);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bounces++;
        if (rigid.velocity.magnitude > 80)
            rigid.velocity = Vector3.ClampMagnitude(rigid.velocity, 80);

        if (Mathf.Abs(rigid.velocity.y) >= 10 && Mathf.Abs(rigid.velocity.x) < 1)
        {
            rigid.velocity += Vector2.right * Random.Range(-rigid.velocity.y, rigid.velocity.y);
        }
        if (Mathf.Abs(rigid.velocity.x) >= 10 && Mathf.Abs(rigid.velocity.y) < 1)
        {
            rigid.velocity += Vector2.up * Random.Range(-rigid.velocity.x, rigid.velocity.x);
        }
        if (bounces > 50)
        {
            rigid.sharedMaterial = material;
            Destroy(gameObject);
        }
    }
}
