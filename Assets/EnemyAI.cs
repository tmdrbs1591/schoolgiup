using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    [SerializeField] float moveSpeed = 3.0f;
    [SerializeField] float detectionRange = 1.0f;
    [SerializeField] float hitRange = 1.0f;
    [SerializeField] int nextMove;
    public bool attacking;
    float attackTime;

    [SerializeField] BoxCollider2D hurtBox;

    [SerializeField] GameObject hitParticle;

    private Transform player;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        InvokeRepeating("Think", 3f, 1.5f); // ���� �ð����� Think �Լ� ȣ��

    }

    IEnumerator Attack() {
        if (attacking) yield break;
        attacking = true;
        bool direction = player.position.x > transform.position.x;
        rigid.velocity = new Vector2((direction?-1:1) * 2, rigid.velocity.y);
        yield return new WaitForSeconds(0.8f);
        hurtBox.gameObject.SetActive(true);
        hurtBox.transform.localPosition = new Vector3((direction?0.5f:-0.5f),0,0);
        for (float i = 1; i > 0; i -= Time.deltaTime) {
            rigid.velocity = new Vector2((direction?1:-1) * i * 8, rigid.velocity.y);
            yield return null;
        }
        hurtBox.gameObject.SetActive(false);
        attacking = false;
        yield break;
    }

    void FixedUpdate()
    {
        if (attacking) return;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            if (player.position.y - transform.position.y > 0.5f && PlayerScript.instance.groundState == -1 && (Mathf.Abs(player.position.x - transform.position.x) <= 5 || rigid.velocity.magnitude < 0.2f))
                rigid.velocity = new Vector2(nextMove * moveSpeed * 1.2f, 15);
            if (Mathf.Abs(rigid.velocity.y) > 0.2f)
                return;
            canBeHurt = true;
            if (Mathf.Abs(player.position.x - transform.position.x) <= hitRange) {
                StartCoroutine(Attack());
                return;
            }
            if (player.position.x > transform.position.x) 
                nextMove = 1;
            else
                nextMove = -1;
        }


        rigid.velocity = new Vector2(nextMove * moveSpeed, rigid.velocity.y);

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        if (!Physics2D.Raycast(frontVec, Vector3.down, 1))
        {
            nextMove *= -1;
            CancelInvoke();
            Invoke("Think", 3f);
        }

        if (nextMove == -1)
        {
            spriteRenderer.flipX = true;
        }
        else if (nextMove == 1)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "WeaponHurtBox")
            Hurt(collision.transform);
    }

    bool canBeHurt = true;
    int health = 2;

    void Update() {
        spriteRenderer.transform.localScale = Vector3.Lerp(spriteRenderer.transform.localScale,new Vector3(1.2479f,1.2479f,1),Time.deltaTime * 5);
    }

    void Hurt(Transform collisionTransform) {
        if (!canBeHurt) return;
        PlayerScript.instance.camShake += 0.2f;
        canBeHurt = false;
        rigid.velocity = new Vector2((collisionTransform.position.x > transform.position.x?4:-4), 5);
        spriteRenderer.transform.localScale /= 2;
        health--;
        Destroy(Instantiate(hitParticle,transform.position,Quaternion.identity), 5);
        if (health <= 0) Destroy(gameObject);
    }

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        Invoke("Think", 1.5f);
    }
}
