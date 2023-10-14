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

    public BoxCollider2D hurtBox;

    public GameObject hitParticle;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = (float)maxHealth;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "WeaponHurtBox")
            Hurt(collision.transform);
    }

    public int maxHealth = 2;
    public int health;

    void Update() {
        spriteRenderer.transform.localScale = Vector3.Lerp(spriteRenderer.transform.localScale,new Vector3(1.2479f,1.2479f,1),Time.deltaTime * 5);
        if (hurtCooldown>0) hurtCooldown -= Time.deltaTime;
    }

    public float hurtCooldown = 0;

    void Hurt(Transform collisionTransform) {
        if (hurtCooldown > 0) return;
        hurtCooldown = 0.5f;
        PlayerScript.instance.camShake = 0.4f;
        rigid.velocity = new Vector2(PlayerScript.instance.dir * 4, 5);
        spriteRenderer.transform.localScale /= 2;
        health--;
        healthBar.value = (float)health / (float)maxHealth;
        Destroy(Instantiate(hitParticle,transform.position,Quaternion.identity), 5);
        if (health <= 0) {
            PlayerScript.instance.Hit(false);
            Destroy(gameObject);
        }
        else {
            PlayerScript.instance.Hit(true);
        }
    }
}
