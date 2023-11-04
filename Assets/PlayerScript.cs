using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine.Utility;
using Cinemachine;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;

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

    [SerializeField]GameObject textEffect;
    [SerializeField] TrailRenderer trail;

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
    float skillCooldown;

    Rigidbody2D rb;

    public float camShake;

    bool[] inputBuffer = new bool[2];

    public int comboCount;
    public float comboTime;

    public float damageOutput;

    public bool skilling = false;

    int currentWeapon = 0;
    public WeaponStat currentWeaponStat;

    void Start()
    {
        instance = this;
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        ChangeWeapon(1);

        
    }

    public void DamageCalculation() {
        crit = (int)Mathf.Min(comboCount,50) >= Random.Range(0, 100);
        damageOutput = currentWeaponStat.damage * (crit ? 3:1) * damageMultiplier;
    }

    bool crit = false;

    public float damageMultiplier = 1;

    public void Hit(Vector3 spawnPosition, float damage, bool kill = false, bool hitstun = true, bool particle = true) {
        if (spawnPosition == Vector3.zero) spawnPosition = weaponSprite.transform.position;
        AudioScript.instance.PlaySound(transform.position,1,Random.Range(0.8f,1.0f),1);
        float shake = 0;
        float stunTime = 0.1f;
        if (spawnPosition == weaponSprite.transform.position)
        {
            movespeed -= 5;
        }
        if (crit)
            {
                if (particle)
                    Destroy(Instantiate(currentWeaponStat.critHitEffect, spawnPosition, Quaternion.identity),2);
                AudioScript.instance.PlaySound(transform.position, 3, Random.Range(0.8f, 1.0f), 1);
                shake += 0.2f;
                stunTime += 0.1f;
            }
            else
        {
            if (particle)
                Destroy(Instantiate(currentWeaponStat.normalHitEffect, spawnPosition, Quaternion.identity), 2);
            }
        if (kill) {
            comboCount++;
            comboTime = 6;
            shake += 0.4f;
        } else {
            comboTime += 1;
            shake += 0.2f;
        }
        HitTextScript tex = Instantiate(textEffect, spawnPosition, Quaternion.identity).GetComponent<HitTextScript>();
        tex.Initialize(damage, crit);
        camShake = shake;
        if (hitstun)
            StartCoroutine(Hitstun(stunTime));
    }

    public void ChangeWeapon(int weapon) {
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

    IEnumerator Skill()
    {
        switch (currentWeapon)
        {
            case 0:
                skilling = true;
                damageMultiplier = 5;
                Time.timeScale = 0.1f;
                for (int i = 0; i < 5; i++)
                {
                    Attack(true, false);
                    yield return new WaitForSecondsRealtime(0.1f);
                }
                Time.timeScale = 1f;
                damageMultiplier = 1;
                skilling = false;
                break;
        }
        yield return null;
    }

    private void Update()
    {
        if (!GameManager.instance.shopping)
        {
            if (Input.GetButtonDown("Jump")) inputBuffer[0] = true;
            if (Input.GetButtonDown("Fire1")) inputBuffer[1] = true;

            if (Input.GetButtonDown("1")) ChangeWeapon(0);
            if (Input.GetButtonDown("2")) ChangeWeapon(1);
            if (Input.GetButtonDown("3")) ChangeWeapon(2);
            if (Input.GetButtonDown("Skill") && skillCooldown <= 0)
            {
                skillCooldown = currentWeaponStat.skillCooldown;
                StartCoroutine(Skill());
            }
        }

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
        if (skillCooldown > 0) skillCooldown -= Time.deltaTime;

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
        camShake = 0.5f;
        health--;
        coolDown = 3;
        state = State.hurt;
        spd.y = 25;

    }

    IEnumerator Attack(bool force = false, bool dash = true) {
        if (attackCooldown > 0 && !force) yield break;
        attackCooldown = currentWeaponStat.cooldown;
        if (currentWeaponStat.projectile == null)
        {
            trail.emitting = true;
            AudioScript.instance.PlaySound(transform.position, currentWeaponStat.soundIndex, Random.Range(0.9f, 1.1f), 1);
            if (dash) movespeed += 20;
            weaponAnimation.SetTrigger("Attack" + (attackOrder + 1));
            attackOrder = (attackOrder + 1) % currentWeaponStat.maxAnimation;
            sprite.transform.localScale = new Vector3(1.3f, 0.8f, 1);
            yield return new WaitForSeconds(currentWeaponStat.attackDelay);
            hurtBox.transform.localPosition = Vector3.right * dir * 1.2f;
            attackTimer = currentWeaponStat.duration;
        } else
        {
            hurtBox.transform.localPosition = Vector3.up * 99999999;
            switch (currentWeaponStat.name)
            {
                default:
                    AudioScript.instance.PlaySound(transform.position, currentWeaponStat.soundIndex, Random.Range(0.9f, 1.1f), 1);
                    Instantiate(currentWeaponStat.projectile, transform.position, Quaternion.identity);
                    break;
                case "Ninja Star":
                    for (int i = 0; i < 3; i++)
                    {
                        AudioScript.instance.PlaySound(transform.position, currentWeaponStat.soundIndex, Random.Range(0.9f, 1.1f), 1);
                        Instantiate(currentWeaponStat.projectile, transform.position, Quaternion.identity);
                        yield return new WaitForSeconds(0.1f);
                    }
                    break;
            }
        }
        yield return null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int moveInput = (int)Input.GetAxisRaw("Horizontal");
        if (GameManager.instance.shopping)
            moveInput = 0;
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
                    StartCoroutine(Attack());
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
                //if (jumped && !Input.GetButton("Jump") && spd.y > 0)
                //{
                //    spd.y -= gravity;
                //}
                if (inputBuffer[0] && jumped)
                {
                    jumped = false;
                    spd.y = jumpSpeed;
                    sprite.transform.localScale = new Vector3(0.7f,1.4f,1);
                    AudioScript.instance.PlaySound(transform.position,0,Random.Range(0.9f,1.1f),1);
                }
                if (inputBuffer[1])
                    StartCoroutine(Attack()); 
                spd.x = movespeed * dir;
                spd.y -= gravity;
                break;
            case State.hurt:
                spd.y -= gravity;
                if (groundState == -1) state = State.normal;
                break;
        }
        rb.velocity = spd;
        if (spd.magnitude < 15)
            trail.emitting = false;

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
        if (collision.tag == "Hurt Projectile") {
            Hurt();
            collision.gameObject.GetComponent<MonoBehaviour>().Invoke("Explode",0);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        groundState = 0;
    }
    
}


