using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkAlpha : MonoBehaviour
{
    Image image;
    void Start()
    {
        image = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        image.color = new Color(1,1,1,(Mathf.Sin(Time.time * 60) + 3) / 4);
    }
}
