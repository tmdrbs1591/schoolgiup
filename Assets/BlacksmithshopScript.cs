using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlacksmithsshopScript : MonoBehaviour
{

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            transform.GetChild(0).gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.C) && GameManager.instance.coin >= 30) {
                AudioScript.instance.PlaySound(PlayerScript.instance.transform.position, 26);
                PlayerScript.instance.currentWeaponStat.damage = Mathf.Round(PlayerScript.instance.currentWeaponStat.damage * 12) / 10;
                GameManager.instance.coin -= 30;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}