using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpitterScript : EnemyBase
{
    [SerializeField] int nextMove;
    public bool attacking;
    float attackCooltime;
    IEnumerator attack;
    [SerializeField] GameObject slime;
    [SerializeField] Sprite openMouth;
    [SerializeField] Sprite closeMouth;

    void Start()
    {
        InvokeRepeating("Think", 3f, 1.5f); // ���� �ð����� Think �Լ� ȣ��

    }

    IEnumerator Attack() {
        if (attacking || dead || attackCooltime > 0) yield break;
        rigid.velocity = new Vector2(0, rigid.velocity.y);
        attackCooltime = 5;
        attacking = true;
        bool direction = PlayerScript.instance.transform.position.x > transform.position.x;
        spriteRenderer.flipX = !direction;
        yield return new WaitForSeconds(0.5f);
        spriteRenderer.transform.localScale = new Vector3(targetScale.x * 0.8f,targetScale.y * 1.2f,1);
        rigid.velocity = new Vector2((direction?1:-1) * -5f, rigid.velocity.y);
        Instantiate(slime,transform.position,Quaternion.identity).GetComponent<Rigidbody2D>().velocity = new Vector2((direction?6:-6),10);
        spriteRenderer.sprite = openMouth;
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.sprite = closeMouth;
        attacking = false;
        attackCooltime = 3;
        yield break;
    }

    void FixedUpdate()
    {
        if (attacking || dead) return;
        float distanceToPlayer = Vector2.Distance(transform.position, PlayerScript.instance.transform.position);
        if (distanceToPlayer <= detectionRange && attackCooltime <= 0)
        {
            if (PlayerScript.instance.transform.position.y - transform.position.y > 0.5f && PlayerScript.instance.groundState == -1 && (Mathf.Abs(PlayerScript.instance.transform.position.x - transform.position.x) <= 5 || rigid.velocity.magnitude < 0.2f))
                rigid.velocity = new Vector2(nextMove * moveSpeed * 1.2f, 20);
            if (Mathf.Abs(rigid.velocity.y) > 0.2f)
                return;
            if (Mathf.Abs(PlayerScript.instance.transform.position.x - transform.position.x) <= hitRange) {
                attack = Attack();
                StartCoroutine(attack);
                return;
            }
                if (PlayerScript.instance.transform.position.x > transform.position.x) 
                    nextMove = 1;
                else
                    nextMove = -1;
        }

        if (attackCooltime > 0) attackCooltime -= Time.fixedDeltaTime;


        rigid.velocity = new Vector2(nextMove * moveSpeed, rigid.velocity.y);

        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.3f, rigid.position.y);
        if (!Physics2D.Raycast(frontVec, Vector3.down, 1))
        {
            nextMove *= -1;
            CancelInvoke();
            Invoke("Think", 3f);
        }


        spriteRenderer.flipX = nextMove == -1;
    }

    void HurtLate() {
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
