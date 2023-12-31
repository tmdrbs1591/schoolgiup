﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlimebossScript : EnemyBase
{
    [SerializeField] int nextMove;
    public bool attacking;
    IEnumerator attack;
    [SerializeField] int slimeType;


    void Start()
    {
        InvokeRepeating("Think", 3f, 1.5f); // ���� �ð����� Think �Լ� ȣ��
        if (rigid.velocity == Vector2.zero)
            rigid.velocity = new Vector2(8, 4);
    }

    IEnumerator Attack()
    {
        if (attacking || dead) yield break;
        attacking = true;
        bool direction = PlayerScript.instance.transform.position.x > transform.position.x;
        spriteRenderer.flipX = !direction;
        rigid.velocity = new Vector2((direction ? -1 : 1) * 2, rigid.velocity.y);
        yield return new WaitForSeconds(0.8f);
        hurtBox.gameObject.SetActive(true);
        hurtBox.transform.localPosition = new Vector3((direction ? 0.5f : -0.5f), 0, 0);
        for (float i = 1; i > 0; i -= Time.deltaTime)
        {
            rigid.velocity = new Vector2((direction ? 1 : -1) * i * 16, rigid.velocity.y);
            if (i < 0.5f) hurtBox.gameObject.SetActive(false);
            yield return null;
        }
        attacking = false;
        yield break;
    }

    float jumpTimer = 2;

    void FixedUpdate()
    {
        if (attacking || dead) return;
        float distanceToPlayer = Vector2.Distance(transform.position, PlayerScript.instance.transform.position);
        if (distanceToPlayer <= detectionRange)
        {
            if (PlayerScript.instance.transform.position.y - transform.position.y > 0.5f && PlayerScript.instance.groundState == -1 && (Mathf.Abs(PlayerScript.instance.transform.position.x - transform.position.x) <= 5 || rigid.velocity.magnitude < 0.2f))
                rigid.velocity = new Vector2(nextMove * moveSpeed * 1.2f, 15);
            if (Mathf.Abs(rigid.velocity.y) > 0.2f)
                return;
            if (Mathf.Abs(PlayerScript.instance.transform.position.x - transform.position.x) <= hitRange)
            {
                attack = Attack();
                StartCoroutine(attack);
                return;
            }
            if (PlayerScript.instance.transform.position.x > transform.position.x)
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

        jumpTimer -= Time.fixedDeltaTime;

        if (jumpTimer <= 0)
        {
            jumpTimer = Random.Range(1f, 3f);
            rigid.velocity = new Vector2(nextMove * moveSpeed * 1.2f, 20f);
        }

        spriteRenderer.flipX = nextMove == -1;
    }

    void DieLate()
    {
        if (slimeType == 3)
        {
            Instantiate(coin, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(-8, 4);
            Instantiate(coin, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(-8, 4);
            Instantiate(coin, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(-8, 4);

        }
        Instantiate(coin, transform.position, Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2(-8, 4);
    }

    void HurtLate()
    {
        if (dead) return;
        StopCoroutine(attack);
        attacking = false;
        hurtBox.gameObject.SetActive(false);
    }

    public void Think()
    {
        if (dead) return;
        nextMove = Random.Range(-1, 2);

        Invoke("Think", 1.5f);
    }
}
