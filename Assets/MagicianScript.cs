using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicianScript : EnemyBase
{
    [SerializeField] int nextMove;
    public bool attacking;
    IEnumerator attack;
    [SerializeField] int slimeType;
    [SerializeField] 
    Vector3[] hidePos;
    public GameObject tpptc;
    [SerializeField]
    int Count;
    [SerializeField]
    float HitTime = 0;
    [SerializeField]
    GameObject tiredptc;

    void Start()
    {
        allowHurt = false;
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
    int jumpCount;

    void FixedUpdate()
    {
        if (attacking || dead) return;

        float distanceToPlayer = Vector2.Distance(transform.position, PlayerScript.instance.transform.position);
        if (distanceToPlayer <= detectionRange)
        {
            if (!allowHurt)
                Move();
        }
        spriteRenderer.flipX = PlayerScript.instance.transform.position.x < transform.position.x;

        if (allowHurt)
        {
           tiredptc.SetActive(true);

        }
        else {
            tiredptc.SetActive(false);
            if (Mathf.Abs(rigid.velocity.y) <= 0.1f)
            {
                jumpCount++;
                jumpCount %= 2;
                rigid.velocity = new Vector2((jumpCount - 0.5f) * 6, 8);
                spriteRenderer.transform.localScale = new Vector3(1, 1.5f, 1);
            }
        }

        rigid.velocity = new Vector2(rigid.velocity.x / 1.1f, rigid.velocity.y - 0.3f);


        if (HitTime > 0) HitTime -= Time.fixedDeltaTime;
        if ((HitTime <= 0 || Count >= 9) && allowHurt)
        {
            allowHurt = false;
            Count = -1;
            Move();
        }
    }

    void DieLate()
    {
        Destroy(healthBar.transform.parent.gameObject);
        for (int i = 0; i < 50; i++)
        {
            Instantiate(coin, transform.position, Quaternion.identity);
        }
    }

    void HurtLate()
    {
        if (dead) return;
        if (!allowHurt)
        Move();
        else
        Count++;
    }

    void Move()
    {
        Count++;
        Vector3 dest = hidePos[Random.Range(0, hidePos.Length)];
        AudioScript.instance.PlaySound(transform.position, 15, Random.Range(0.8f, 1.0f), 1);
        Destroy(Instantiate(tpptc, transform.position , Quaternion.identity),2f);
        while (Vector3.Distance(transform.parent.position + dest,PlayerScript.instance.transform.position) <= detectionRange + 5)   
        {
            dest = hidePos[Random.Range(0, hidePos.Length)];
           
        }
        transform.localPosition = dest;
        if (Count >= 5)
        {
            allowHurt = true;
            HitTime = 5;
        }
    }
    
}
