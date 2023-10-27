using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTowerScript : MonoBehaviour

     
{
    bool entered;
    public GameObject healeffect; 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        entered = true;
        transform.GetChild(0).gameObject.SetActive(true);
    }   

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.C) && entered)
        {
            AudioScript.instance.PlaySound(PlayerScript.instance.transform.position, 11);
            PlayerScript.instance.health = 8;
            Destroy(Instantiate(healeffect, PlayerScript.instance.transform.position, Quaternion.identity), 3f);


        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        entered = false;
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
