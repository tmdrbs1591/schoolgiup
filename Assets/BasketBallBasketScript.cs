using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketBallBasketScript : MonoBehaviour
{
    bool checking;
    BoxCollider2D box;
    BasketBallArenaScript bbas;
    [SerializeField] bool playerHoop;
    [SerializeField]
    GameObject goalptc;
    [SerializeField] GameObject coin;

    private void Start()
    {
        bbas = transform.parent.GetComponent<BasketBallArenaScript>();
        box = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.y < 0) checking = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody2D>().velocity.y < 0 && checking)
        {
            bbas.Goal(playerHoop);
            Destroy(Instantiate(goalptc,transform.position,Quaternion.identity),2f);
            AudioScript.instance.PlaySound(transform.position, 22, 1, 1);
            if (coin != null)
            {
                for (int i = 0; i < 10; i++)
                {
                    Instantiate(coin, transform.position, Quaternion.identity);
                }
            }
        }
        checking = false;
    }
}
