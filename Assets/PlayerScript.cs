using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine.Utility;
using Cinemachine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript instance;

    [SerializeField] float gravity;
    [SerializeField] float speed;
    [SerializeField] float jumpSpeed;

    public int health = 3;

    [SerializeField]SpriteRenderer sprite;
    [SerializeField]GameObject weaponHolder;
    [SerializeField]SpriteRenderer weaponSprite;
    [SerializeField]Animator weaponAnimation;
    [SerializeField]BoxCollider2D hurtBox;
    BoxCollider2D boxCollider;
    Camera cam;
    [SerializeField]CinemachineCameraOffset offset;

    [SerializeField]GameObject hitEffect;
    [SerializeField]GameObject critEffect;

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
    public int dir = 1;
    float movespeed;
    bool jumped = false;
    float attackTimer;
    float attackCooldown;
    int attackOrder;

    Rigidbody2D rb;

    public float camShake;

    bool[] inputBuffer = new bool[2];

    public int comboCount;
    public float comboTime;

    public float damageOutput;

    int currentWeapon = 0;
    WeaponStat currentWeaponStat;

    void Start()
    {
        instance = this;
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        ChangeWeapon(0);
    }

    public void DamageCalculation() {
        crit = (int)Random.Range(Mathf.Min(comboCount,50),60) >= 55;
        damageOutput = currentWeaponStat.damage * (crit ? 3:1);
    }

    bool crit = false;

    public void Hit(bool kill = false) {
        AudioScript.instance.PlaySound(transform.position,1,Random.Range(0.8f,1.0f),1);
        float shake = 0;
        float stunTime = 0.1f;
        if (crit) {
            Instantiate(critEffect,weaponSprite.transform.position,Quaternion.identity);
            AudioScript.instance.PlaySound(transform.position,3,Random.Range(0.8f,1.0f),1);
            shake += 0.2f;
            stunTime += 0.1f;
        }
        else {
            Instantiate(hitEffect,weaponSprite.transform.position,Quaternion.identity);
        }
        if (kill) {
            comboCount++;
            comboTime = 6;
            shake += 0.4f;
        } else {
            comboTime += 1;
            shake += 0.2f;
        }
        camShake = shake;
        movespeed -= 5;
        StartCoroutine(Hitstun(stunTime));
    }

    void ChangeWeapon(int weapon) {
        currentWeapon = weapon;
        int i = 0;
        foreach (Transform a in weaponHolder.transform) {
            a.gameObject.SetActive(i == currentWeapon);
            i++;
        }
        currentWeaponStat = weaponHolder.transform.GetChild(weapon).GetComponent<WeaponStat>();
        weaponSprite = weaponHolder.transform.GetChild(weapon).GetComponent<SpriteRenderer>();
        weaponAnimation = weaponHolder.transform.GetChild(weapon).GetComponent<Animator>();
        attackOrder = (attackOrder + 1) % currentWeaponStat.maxAnimation;
    }

    IEnumerator Hitstun(float seconds) {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(seconds);
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump")) inputBuffer[0] = true;
        if (Input.GetButtonDown("Fire1")) inputBuffer[1] = true;

        if (Time.timeScale != 0)
            offset.m_Offset = new Vector3(Random.Range(-camShake,camShake),Random.Range(-camShake,camShake),0);

        sprite.transform.localScale = Vector3.Lerp(sprite.transform.localScale,Vector3.one,Time.deltaTime * 5);
        sprite.color = new Color(1,1,1,(coolDown>0?0.4f:1));
        sprite.flipX = dir == -1;

        if (coolDown>0) coolDown -= Time.deltaTime;
        
        weaponHolder.transform.localScale = new Vector3(dir,1,1);
        weaponHolder.transform.eulerAngles = Vector3.forward * -spd.y * dir;

        if (attackTimer > 0) attackTimer -= Time.deltaTime;
        if (attackCooldown > 0) attackCooldown -= Time.deltaTime;

        hurtBox.gameObject.SetActive(attackTimer > 0);

        if (camShake > 0) {
            camShake -= Time.deltaTime;
            if (camShake <= 0)
                camShake = 0;
        }
        
        if (comboCount > 0) {
            comboTime -= Time.deltaTime;
            if (comboTime <= 0) comboCount = 0;
        }
    }

    public void Hurt() {
        if (coolDown > 0) return;
        PlayerScript.instance.camShake = 0.4f;
        health--;
        coolDown = 3;
        state = State.hurt;
        spd.y = 25;
    }

    void Attack() {
        if (attackCooldown > 0) return;
        AudioScript.instance.PlaySound(transform.position,2,Random.Range(0.9f,1.1f),1);
        hurtBox.transform.localPosition = Vector3.right * dir * 1.2f;
        movespeed += 20;
        attackTimer = currentWeaponStat.duration;
        attackCooldown = currentWeaponStat.cooldown;
        weaponAnimation.SetTrigger("Attack" + (attackOrder + 1));
        attackOrder = (attackOrder + 1) % currentWeaponStat.maxAnimation;
        sprite.transform.localScale = new Vector3(1.3f,0.8f,1);
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
                    AudioScript.instance.PlaySound(transform.position,0,Random.Range(0.9f,1.1f),1);
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
                        movespeed -= 1f;
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
                        movespeed -= 1f;
                if (jumped && !Input.GetButton("Jump") && spd.y > 0)
                {
                    spd.y -= gravity;
                }
                if (inputBuffer[0] && jumped)
                {
                    jumped = false;
                    spd.y = jumpSpeed;
                    sprite.transform.localScale = new Vector3(0.7f,1.4f,1);
                    AudioScript.instance.PlaySound(transform.position,0,Random.Range(0.9f,1.1f),1);
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


