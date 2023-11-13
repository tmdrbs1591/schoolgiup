using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainWeaponImageSetter : MonoBehaviour
{
    Image imager;

    void Start()
    {
        imager = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        imager.sprite = GameManager.instance.weaponSprites[PlayerScript.instance.currentWeapon];
    }
}
