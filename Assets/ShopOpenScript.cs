using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopOpenScript : MonoBehaviour
{
    bool inside = false;
    bool opened = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        inside = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && inside)
        {
            GameManager.instance.OpenShop();
            if (!opened)
            {
                opened = true;
                ShopManager.instance.Reroll();
            }

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inside = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
