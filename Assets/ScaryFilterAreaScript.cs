using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaryFilterAreaScript : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            GameManager.instance.inScary = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            GameManager.instance.inScary = false;
    }
}
