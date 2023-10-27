using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombostopScript : MonoBehaviour
{

    bool stayCheck;

     void Update()
    {
       
        if (stayCheck)
        {
            PlayerScript.instance.comboTime += Time.deltaTime;
        }
    }


    void Awake()
    {
       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            
            stayCheck = true;
        }
            


    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
           
            stayCheck = false;
        }
            
    }
}
