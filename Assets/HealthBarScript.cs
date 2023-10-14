using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarScript : MonoBehaviour
{
    [SerializeField] GameObject heartUnit;
    int oldHealth;

    // Update is called once per frame
    void Update()
    {
        if (oldHealth != PlayerScript.instance.health) {
            while (oldHealth != PlayerScript.instance.health) {
                if (oldHealth > PlayerScript.instance.health) {
                    oldHealth--;
                    transform.GetChild(oldHealth).gameObject.GetComponent<HeartScript>().Die();
                } else {
                    oldHealth++;
                    Instantiate(heartUnit,transform);
                }
            }
        }
    }
}
