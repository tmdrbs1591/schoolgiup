using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    public float moveSpeed = 3.0f;
    public float detectionRange = 1.0f;
    public int nextMove;


    private Transform player;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        InvokeRepeating("Think", 3f, 1.5f); // ���� �ð����� Think �Լ� ȣ��


    }

    void FixedUpdate()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            // �÷��̾ ���� ���� ���� ������ �÷��̾� ������ �̵�
            if (player.position.x > transform.position.x)
                nextMove = 1;
            else
                nextMove = -1;
        }


        rigid.velocity = new Vector2(nextMove * moveSpeed, rigid.velocity.y);

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));
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
