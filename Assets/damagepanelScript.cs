using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class damagepanelScript : MonoBehaviour
{
    int health;
    Image panel;

    void Flash()
    {
        panel.color = new Color(1, 0, 0, 0.2f);
    }

    private void Start()
    {
        panel = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health != PlayerScript.instance.health)
        {
            if (PlayerScript.instance.health < health) Flash();
            health = PlayerScript.instance.health;
        }
        if (panel.color.a > 0) panel.color -= new Color(0, 0, 0, Time.deltaTime);
    }
}
