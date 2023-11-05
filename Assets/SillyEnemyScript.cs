using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SillyEnemyScript : EnemyBase
{
    [SerializeField] int nextMove;
    public bool attacking;
    IEnumerator attack;

    [SerializeField] Sprite[] sprites;

    void Start()
    {
        InvokeRepeating("Think", 3f, 1.5f); // ���� �ð����� Think �Լ� ȣ��

    }

    IEnumerator Attack() {
        if (attacking || dead) yield break;
        attacking = true;
        PlayerScript.instance.camShake += 0.5f;
        spriteRenderer.sprite = sprites[1];
        AudioScript.instance.PlaySound(transform.position, 18, Random.Range(0.7f, 1.1f), 1,transform);
        bool direction = PlayerScript.instance.transform.position.x < transform.position.x;
        spriteRenderer.flipX = !direction;
        rigid.velocity = new Vector2((direction?-1:1) * 2f, rigid.velocity.y);
        hurtBox.gameObject.SetActive(true);
        hurtBox.transform.localPosition = new Vector3((direction?0.5f:-0.5f),0,0);
        while (Mathf.Abs(rigid.velocity.x) > 0.1f) {
            rigid.velocity += Vector2.right * (direction?-0.5f:0.5f);
            yield return new WaitForFixedUpdate();
        }
        Die();
        yield break;
    }

    void FixedUpdate()
    {
        if (attacking || dead) return;
        float distanceToPlayer = Vector2.Distance(transform.position, PlayerScript.instance.transform.position);
        if (distanceToPlayer <= detectionRange)
        {
            spriteRenderer.transform.localPosition = new Vector3(Random.Range(-0.05f,0.05f),Random.Range(-0.05f,0.05f),1);
            spriteRenderer.flipX = transform.position.x < PlayerScript.instance.transform.position.x;
            if (Mathf.Abs(PlayerScript.instance.transform.position.x - transform.position.x) <= hitRange) {
                attack = Attack();
                StartCoroutine(attack);
                return;
            }
        } else {
            spriteRenderer.transform.localPosition = new Vector3(0,0,1);
        }
    }

    void HurtLate() {
        if (dead) return;
        StopCoroutine(attack);
        attacking = false;
        hurtBox.gameObject.SetActive(false);
    }
}
