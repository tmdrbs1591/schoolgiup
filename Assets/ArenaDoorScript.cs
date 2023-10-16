using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaDoorScript : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "WeaponHurtBox")
            Hurt();
    }  

    [SerializeField] GameObject[] arenas;

    int health = 2;
    
    float hurtCooldown = 0;
    
    void Update() {
        if (hurtCooldown>0) hurtCooldown -= Time.deltaTime;
    }

    void Hurt() {
        if (hurtCooldown > 0) return;
        hurtCooldown = 0.5f;
        PlayerScript.instance.camShake = 0.4f;
        health--;
        if (health <= 0) {
            PlayerScript.instance.comboTime = 6;
            Instantiate(arenas[Random.Range(0,arenas.Length)],transform.parent.parent).transform.position = transform.position + Vector3.right / 2;
            Destroy(gameObject);
            GameManager.instance.doorsBroken++;
        }
    }
}
