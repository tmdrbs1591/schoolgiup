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
     public Transform healpos;
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
    public GameObject lighting;

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
    float attackBuffer;
    int attackOrder;
    float skillCooldown;

    Rigidbody2D rb;

    public float camShake;

    bool[] inputBuffer = new bool[1];

    public int comboCount;
    public float comboTime;

    public float damageOutput;

    public bool skilling = false;

    public int currentWeapon = 0;
    public int otherWeapon = -1;
    public bool GetAcom = false;
    public WeaponStat currentWeaponStat;

    void Start()
    {
        instance = this;
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        ChangeWeapon(0);
    }

    public void DamageCalculation() {
        crit = (int)Mathf.Min(comboCount,50) >= Random.Range(0, 100);
        damageOutput = currentWeaponStat.damage * (crit ? 2:1) * damageMultiplier;
    }

    bool crit = false;

    public float damageMultiplier = 1;

    public void Hit(Vector3 spawnPosition, float damage, bool kill = false, bool hitstun = true, bool particle = true) {
        if (spawnPosition == Vector3.zero) spawnPosition = weaponSprite.transform.position;
        AudioScript.instance.PlaySound(transform.position,1,Random.Range(0.8f,1.0f),1);
        float shake = 0;
        float stunTime = 0.06f;
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
                stunTime += 0.07f;
            }
            else
        {
            if (particle)
                Destroy(Instantiate(currentWeaponStat.normalHitEffect, spawnPosition, Quaternion.identity), 2);
            }
        if (kill) {
            comboCount++;
            comboTime = 6;
            shake += 0.3f;
            if (GameManager.instance.maxCombo < comboCount) GameManager.instance.maxCombo = comboCount;
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
        if (GameManager.instance.gameOver) return;
        if (!GameManager.instance.shopping)
        {
            if (Input.GetButtonDown("Jump")) inputBuffer[0] = true;
            if (Input.GetButtonDown("Fire1")) attackBuffer = 0.3f;

            //if (Input.GetButtonDown("1")) ChangeWeapon(0);
            //if (Input.GetButtonDown("2")) ChangeWeapon(1);
            //if (Input.GetButtonDown("3")) ChangeWeapon(2);

            if (Input.GetButtonDown("Skill") && otherWeapon != -1)
            {
                int keep = currentWeapon;
                ChangeWeapon(otherWeapon);
                otherWeapon = keep;
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
        if (attackBuffer > 0) attackBuffer -= Time.deltaTime;

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
        AudioScript.instance.PlaySound(transform.position, 25, Random.Range(0.8f, 1.0f), 1);
        camShake = 0.5f;
        health--;
        coolDown = 1;
        state = State.hurt;
        spd.y = 25;
        if (health <= 0) {
            camShake = 2f;
            Time.timeScale = 0.2f;
            GameManager.instance.gameOver = true;
            rb.freezeRotation = false;
            rb.angularVelocity = Random.Range(-500,500);
            rb.gravityScale = 6;
            rb.velocity = new Vector3(Random.Range(-10f,10f),20,0);
            boxCollider.enabled = false;
        }
    }

    IEnumerator Attack(bool force = false, bool dash = true) {
        if (attackCooldown > 0 && !force) yield break;
        attackBuffer = 0;
        attackCooldown = currentWeaponStat.cooldown;
        if (currentWeaponStat.projectile == null)
        {
            trail.emitting = true;
            AudioScript.instance.PlaySound(transform.position, currentWeaponStat.soundIndex, Random.Range(0.9f, 1.1f), 1);
            weaponAnimation.SetTrigger("Attack" + (attackOrder + 1));
            attackOrder = (attackOrder + 1) % currentWeaponStat.maxAnimation;
            sprite.transform.localScale = new Vector3(1.3f, 0.8f, 1);
            yield return new WaitForSeconds(currentWeaponStat.attackDelay);
            if (dash) movespeed += 14;
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
        if (GameManager.instance.gameOver) return;
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
                if (attackBuffer > 0)
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
                if (attackBuffer > 0)
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


