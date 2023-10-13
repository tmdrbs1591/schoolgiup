using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    static PlayerScript instance;

    [SerializeField] float gravity;
    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;

    int health = 3;

    SpriteRenderer sprite;
    BoxCollider2D boxCollider;
    Camera cam;

    Vector2 spd;
    float coolDown;

    enum State
    {
        normal,
        air,
        attack,
        hurt
    }

    [SerializeField] State state = State.normal;

    [SerializeField] int groundState = 0;
    int dir = 1;
    float movespeed;
    bool jumped = false;

    Rigidbody2D rb;

    bool[] inputBuffer = new bool[2];

    void Start()
    {
        instance = this;
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump")) inputBuffer[0] = true;
        if (Input.GetButtonDown("Fire1")) inputBuffer[1] = true;
        cam.transform.position = Vector3.Lerp(cam.transform.position, new Vector3(transform.position.x, transform.position.y + 1, cam.transform.position.z), Time.deltaTime * 10);
        sprite.transform.localScale = Vector3.Lerp(sprite.transform.localScale,Vector3.one,Time.deltaTime * 5);
        if (coolDown>0) coolDown -= Time.deltaTime;
        sprite.flipX = dir == -1;
    }

    public void Hurt() {
        if (coolDown > 0) return;
        state = State.hurt;
        spd.y = 5;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int moveInput = (int)Input.GetAxisRaw("Horizontal");
        switch (state)
        {
            case State.normal:
                spd.y = 0;
                jumped = false;
                if (groundState == 0) state = State.air; 
                if (inputBuffer[0])
                {
                    state = State.air;
                    spd.y = jumpSpeed;
                    groundState = 0;
                    jumped = true;
                    sprite.transform.localScale = new Vector3(0.7f,1.4f,1);
                }
                if (moveInput == 0)
                {
                    if (movespeed < 0)
                        movespeed = 0;
                    else if (movespeed > 0)
                        movespeed -= 1f;
                } else {
                    if (dir != moveInput)
                    {
                        dir = moveInput;
                        movespeed = speed;
                    }
                    if (movespeed < 15)
                        movespeed += speed;
                }
                spd.x = movespeed * dir;
                break;
            case State.air:
                if (groundState == -1) state = State.normal;
                if (moveInput != 0)
                {
                    if (dir != moveInput)
                    {
                        dir = moveInput;
                        movespeed = speed;
                    }
                    if (movespeed < 12)
                        movespeed += speed;
                }
                if (jumped && !Input.GetButton("Jump") && spd.y > 0)
                {
                    spd.y -= gravity;
                }
                if (inputBuffer[0] && jumped)
                {
                    jumped = false;
                    spd.y = jumpSpeed;
                }
                spd.x = movespeed * dir;
                spd.y -= gravity;
                break;
            case State.hurt:
                spd.y -= gravity;
                if (groundState == -1) state = State.normal;
                break;
        }
        rb.velocity = spd;

        for (int i = 0; i < inputBuffer.Length; i++) inputBuffer[i] = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (Mathf.Abs(rb.velocity.y) < 0.03f) //check velocity.y is 0
            groundState = (spd.y > 0 ? 1 : -1);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "HurtBox")
            Hurt();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        groundState = 0;
    }
}


