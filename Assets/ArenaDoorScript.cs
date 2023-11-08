using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaDoorScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Im Getting Hit Ow ow ");
        if (collision.tag == "WeaponHurtBox")
            Hurt();
    }  

    [SerializeField] GameObject[] arenas;
    [SerializeField] GameObject[] bossArenas;
    [SerializeField] GameObject[] randomArenas;
    [SerializeField] GameObject shop;
    [SerializeField] GameObject dorrbreakpaticle;
    int health = 3;
    float shake;
    

    
    float hurtCooldown = 0;
    
    void Update()
    {
        if (hurtCooldown > 0) hurtCooldown -= Time.deltaTime;
        if (shake > 0) shake -= Time.deltaTime;
        if (shake < 0) shake = 0;
        transform.GetChild(0).localPosition = new Vector2(Random.Range(-shake * 0.4f, shake * 0.4f), + Random.Range(-shake * 0.4f, shake * 0.4f));
    }

    void Hurt() {
        //if (hurtCooldown > 0) return;
        hurtCooldown = 0.5f;
        PlayerScript.instance.camShake = 0.4f;
        AudioScript.instance.PlaySound(transform.position, 19, Random.Range(0.8f, 1.0f), 1);
        health--;
        Destroy(Instantiate(dorrbreakpaticle, transform.position, Quaternion.identity), 5f);
        shake = 0.5f;
        if (health <= 0) {
            PlayerScript.instance.comboTime = 6;
            GameManager.instance.randomArenaLeft--;
            SpawnNextArena();
            Destroy(gameObject);
            Destroy(Instantiate(dorrbreakpaticle, transform.position, Quaternion.identity),5f);
            GameManager.instance.doorsBrokenTotal++;
        }
    }

    void SpawnNextArena()
    {
        if (GameManager.instance.randomArenaLeft <= 0 && (GameManager.instance.doorsBroken - 1) % 10 != 0)
        {
            Instantiate(randomArenas[Random.Range(0, randomArenas.Length)], transform.parent.parent).transform.position = transform.position + Vector3.right / 2;
            GameManager.instance.randomArenaLeft = Random.Range(2, 4);
        }
        else
        {
            GameManager.instance.doorsBroken++;
            if ((GameManager.instance.doorsBroken) % 5 == 0)
            {
                Instantiate(shop, transform.parent.parent).transform.position = transform.position + Vector3.right / 2;
            }
            else if ((GameManager.instance.doorsBroken - 1) % 10 == 0 && GameManager.instance.doorsBroken != 1)
            {
                Instantiate(bossArenas[Mathf.Min(GameManager.instance.boss, bossArenas.Length)], transform.parent.parent).transform.position = transform.position + Vector3.right / 2;
                GameManager.instance.boss++;
            }
            else
            {
                GameObject arenaToSpawn = arenas[Random.Range(0, arenas.Length)];
                while (arenaToSpawn == transform.parent.gameObject)
                    arenaToSpawn = arenas[Random.Range(0, arenas.Length)];
                Instantiate(arenaToSpawn, transform.parent.parent).transform.position = transform.position + Vector3.right / 2;
            }
        }
        
    }
  
}
