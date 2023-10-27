using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithsshopScript : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
       
        transform.GetChild(0).gameObject.SetActive(true);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}