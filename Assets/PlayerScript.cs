using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript instance;

    [SerializeField] float gravity;
    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;

    int health = 3;

    [SerializeField]SpriteRenderer sprite;
    [SerializeField]SpriteRenderer weaponSprite;
    [SerializeField]BoxCollider2D hurtBox;
    [SerializeField]Animator weaponAnimation;
    BoxCollider2D boxCollider;
    Camera cam;

    Vector2 spd;
    float coolDown;

    enum State
    {
        normal,
        air,
        hurt
    }

    [SerializeField] State state = State.normal;

    public int groundState = 0;
    int dir = 1;
    float movespeed;
    bool jumped = false;
    float attackTimer;
    float attackCooldown;
    int attackOrder;

    Rigidbody2D rb;

    Vector3 cameraPosition;
    public float camShake;

    bool[] inputBuffer = new bool[2];

    void Start()
    {
        instance = this;
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump")) inputBuffer[0] = true;
        if (Input.GetButtonDown("Fire1")) inputBuffer[1] = true;

        //cameraPosition = Vector3.Lerp(cameraPosition, new Vector3(transform.position.x + spd.x / 5, transform.position.y + 1, cam.transform.position.z), Time.deltaTime * 10);
        //cam.transform.position = cameraPosition + new Vector3(Random.Range(-camShake,camShake),Random.Range(-camShake,camShake),0);

        sprite.transform.localScale = Vector3.Lerp(sprite.transform.localScale,Vector3.one,Time.deltaTime * 5);
        sprite.color = new Color(1,1,1,(coolDown>0?0.4f:1));
        sprite.flipX = dir == -1;

        if (coolDown>0) coolDown -= Time.deltaTime;
        
        weaponSprite.flipX = dir == -1;
        weaponSprite.transform.localPosition = new Vector3(0.7f * dir, -0.2f, 0.1f);
        weaponSprite.transform.eulerAngles = Vector3.forward * -spd.y * dir;

        if (attackTimer > 0) attackTimer -= Time.deltaTime;
        if (attackCooldown > 0) attackCooldown -= Time.deltaTime;

        hurtBox.gameObject.SetActive(attackTimer > 0);

        if (camShake > 0) {
            camShake -= Time.deltaTime;
            if (camShake <= 0)
                camShake = 0;
        }
    }

    public void Hurt() {
        if (coolDown > 0) return;
        coolDown = 3;
        state = State.hurt;
        spd.y = 25;
    }

    void Attack() {
        if (attackCooldown > 0) return;
        hurtBox.transform.localPosition = Vector3.right * dir;
        movespeed += 5;
        attackTimer = 0.1f;
        attackCooldown = 0.3f;
        weaponAnimation.SetTrigger("Attack" + (attackOrder + 1));
        attackOrder = (attackOrder + 1) % 3;
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
                if (inputBuffer[1])
                    Attack();
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
                    if (movespeed < 12)
                        movespeed += speed;
                    if (movespeed > 15)
                        movespeed -= 0.25f;
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
                        movespeed = 0;
                    }
                    if (movespeed < 12)
                        movespeed += speed;
                }
                    if (movespeed > 15)
                        movespeed -= 0.25f;
                if (jumped && !Input.GetButton("Jump") && spd.y > 0)
                {
                    spd.y -= gravity;
                }
                if (inputBuffer[0] && jumped)
                {
                    jumped = false;
                    spd.y = jumpSpeed;
                    sprite.transform.localScale = new Vector3(0.7f,1.4f,1);
                }
                if (inputBuffer[1])
                    Attack();
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


