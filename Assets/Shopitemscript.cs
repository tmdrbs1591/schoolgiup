using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Shopitemscript : MonoBehaviour
{
    [SerializeField]
    public int price = 1000;
    [SerializeField]
    TMP_Text pricetag;
     [SerializeField]
    int weapon;
    void Start()
    {
        pricetag.text = $"{price}";
    }

   
    void Update()
    {
      
    }
    public void Buy()
    {
        if (GameManager.instance.coin >= price)
        {
            AudioScript.instance.PlaySound(PlayerScript.instance.transform.position,9);
            GameManager.instance.coin -= price;
            if (PlayerScript.instance.otherWeapon == -1)
                PlayerScript.instance.otherWeapon = weapon;
            else {
                PlayerScript.instance.ChangeWeapon(weapon);
            }
        }
    }
}
