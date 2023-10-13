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

    [SerializeField] BoxCollider2D hurtBox;

    private Transform player;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        InvokeRepeating("Think", 3f, 1.5f); // ���� �ð����� Think �Լ� ȣ��

    }

    void Attack() {
        if (attacking) return;
        attacking = true;
        rigid.velocity = new Vector2(nextMove * moveSpeed * 3, rigid.velocity.y);
    }

    void FixedUpdate()
    {
        hurtBox.gameObject.SetActive(attacking);
        if (attacking) {
            rigid.velocity = new Vector2(rigid.velocity.x / 1.05f, rigid.velocity.y);
            if (rigid.velocity.x <= 0.5f)
                attacking = false;
            return;
        }
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            // �÷��̾ ���� ���� ���� ������ �÷��̾� ������ �̵�
            if (player.position.x > transform.position.x)
                nextMove = 1;
            else
                nextMove = -1;

            if (Mathf.Abs(player.position.x - transform.position.x) <= hitRange && Mathf.Abs(player.position.y - transform.position.y) <= 1) {
                attacking = true;
            }
        }


        rigid.velocity = new Vector2(nextMove * moveSpeed, rigid.velocity.y);

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1);
        if (rayHit.collider == null)
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

    void Think()
    {
        nextMove = Random.Range(-1, 2);

        Invoke("Think", 1.5f);
    }
}
