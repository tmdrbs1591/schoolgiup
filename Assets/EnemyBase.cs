using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour
{
    public Rigidbody2D rigid;
    public SpriteRenderer spriteRenderer;
    public float moveSpeed = 3.0f;
    public float detectionRange = 1.0f;
    public float hitRange = 1.0f;
    public float attackTime;
    public Slider healthBar;
    public int scoreAward;
    public int dieSoundIndex;
    public int bloodSoundIndex;
    public GameObject coin;

    public BoxCollider2D hurtBox;

    public GameObject hitParticle;
    public GameObject bloodParticle;

    public bool dead = false;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = (float)maxHealth;
        hurtCooldown = 0.5f;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (dead) return;
        if (collision.tag == "WeaponHurtBox")
            Hurt(collision.transform);
    }

    public int maxHealth = 2;
    public float health;
    public float burnDamage;
    float burnTimer;

    void Update()
    {
        spriteRenderer.transform.localScale = Vector3.Lerp(spriteRenderer.transform.localScale, new Vector3(1.2479f, 1.2479f, 1), Time.deltaTime * 5);
        if (dead)
        {
            if (rigid.velocity.y <= -100) Destroy(gameObject);
            return;
        }
        if (hurtCooldown>0) hurtCooldown -= Time.deltaTime;
        if (burnDamage > 0)
        {
            burnDamage -= Time.deltaTime;
            if (burnTimer <= 0)
            {
                burnTimer += 0.1f;
                Hurt(transform,true,1f);
            }
            burnTimer -= Time.deltaTime;
        }
    }

    public float hurtCooldown = 0;

    void Hurt(Transform collisionTransform, bool forceDamage = false, float damage = 0) {
        Vector3 spawnPos = Vector3.zero;
        bool hitstun = true;
        if (hurtCooldown > 0 && !forceDamage) return;
        spriteRenderer.transform.localScale /= 2;
        if (collisionTransform == transform)
        {
            spawnPos = transform.position;
            hitstun = false;
        }
        if (damage == 0)
        {
            rigid.velocity = new Vector2(PlayerScript.instance.dir * 4, 5);
            hurtCooldown = 0.5f;
            PlayerScript.instance.DamageCalculation();
            health -= PlayerScript.instance.damageOutput;
            if (PlayerScript.instance.currentWeaponStat.burn)
            {
                burnDamage = 1.3f;
                bloodParticle = PlayerScript.instance.currentWeaponStat.bleedEffect;
            }
            damage = PlayerScript.instance.damageOutput;
        } else
        {
            health -= damage;
        }
        healthBar.value = (float)health;
        Destroy(Instantiate(hitParticle,transform.position,Quaternion.identity), 5);
        if (burnDamage > 0) {
            Destroy(Instantiate(bloodParticle, transform.position, Quaternion.identity), 5);
            AudioScript.instance.PlaySound(transform.position, bloodSoundIndex, Random.Range(0.8f, 1.0f));
        }
        if (health <= 0) {
            PlayerScript.instance.Hit(spawnPos,damage,true,hitstun);
            healthBar.gameObject.SetActive(false);
            GameManager.instance.AddScore(scoreAward, true);
            AudioScript.instance.PlaySound(transform.position,dieSoundIndex, Random.Range(0.8f, 1.0f));
            dead = true;
            rigid.velocity = rigid.velocity = new Vector2(PlayerScript.instance.dir * 5, 7);
            rigid.constraints = RigidbodyConstraints2D.None;
            rigid.angularVelocity = Random.Range(-100, 100);
            GetComponent<Collider2D>().enabled = false;
            Instantiate(coin,transform.position,Quaternion.identity);
        }
        else {
            PlayerScript.instance.Hit(spawnPos, damage, false, hitstun);
        }
    }
}
