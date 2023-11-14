using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondaryWeaponHolderImageScript : MonoBehaviour
{
    Image imager;

    void Start()
    {
        imager = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerScript.instance.otherWeapon == -1)
            imager.enabled = false;
        else {
            imager.enabled = true;
            imager.sprite = GameManager.instance.weaponSprites[PlayerScript.instance.otherWeapon];
        }
    }
}
