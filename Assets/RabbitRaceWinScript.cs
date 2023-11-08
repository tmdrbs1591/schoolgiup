using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static Unity.Collections.AllocatorManager;

public class RabbitRaceWinScript : MonoBehaviour
{
    bool waiting;
    bool win;
    [SerializeField] GameObject winParticle;
    [SerializeField] TMP_Text bunnywintext;
    [SerializeField] TMP_Text playerwintext;
    [SerializeField] GameObject coin;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bunny"))
            other.gameObject.GetComponent<RabbitRaceScript>().direction = 0;

        if (waiting) return;

        if (other.gameObject.CompareTag("Bunny"))
        {
            win = false;
            waiting = true;
            Instantiate(winParticle, transform.position, Quaternion.identity);
            bunnywintext.gameObject.SetActive(true);

        }
        if (other.gameObject.CompareTag("Player"))
        {
            win = true;
            waiting = true;
            Instantiate(winParticle, transform.position, Quaternion.identity);
            playerwintext.gameObject.SetActive(true);
            AudioScript.instance.PlaySound(transform.position, 22, 1, 1);
            for (int i = 0; i < 15; i++)
            {
                Instantiate(coin, transform.position, Quaternion.identity);
            }
        }
    }
}
