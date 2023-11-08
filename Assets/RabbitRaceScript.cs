using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RabbitRaceScript : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer sprite;
    public int groundState;
    public int direction = 0;
    public float Countdown = 2;
    int count = 4;
    [SerializeField] TMP_Text countText;
    [SerializeField] GameObject blocker;
    [SerializeField] GameObject blockerBreakParticle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        countText.color = new Color(1, 1, 1, countText.color.a - Time.fixedDeltaTime);
        if (groundState == -1)
        {
            float jumpHeight = 9;
            if (Physics2D.Raycast(transform.position, Vector3.right * direction, 10,6)) jumpHeight = 18;
            rb.velocity = new Vector2(direction * 30,jumpHeight);
            sprite.flipX = direction > 0;
            sprite.transform.localScale = new Vector3(0.9f, 1.2f, 1);
        }
        rb.velocity = new Vector2(rb.velocity.x / 1.02f,rb.velocity.y);
        sprite.transform.localScale = Vector3.Lerp(sprite.transform.localScale,Vector3.one,Time.fixedDeltaTime*5);

        if (Countdown > 0 )
            Countdown -= Time.fixedDeltaTime;

        if (Countdown <= 0 && count > 0)
        {
            count--;
            if (count > 0)
            {
                Countdown = 1;
                countText.text = $"{count}..";
                countText.fontSize = 100 + (3-count) * 30;
                countText.color = Color.white;
                AudioScript.instance.PlaySound(blocker.transform.position, 20, 1, 1);
            }
            else
            {
                direction = 1;
                countText.text = "GO!!";
                countText.color = Color.white;
                AudioScript.instance.PlaySound(blocker.transform.position, 21, 1, 1);
                Instantiate(blockerBreakParticle, blocker.transform.position, Quaternion.identity);
                Destroy(blocker);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Mathf.Abs(rb.velocity.y) < 0.1f) //check velocity.y is 0
            groundState = -1;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        groundState = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BunnyTurnTrigger")) direction *= -1;
    }
}
