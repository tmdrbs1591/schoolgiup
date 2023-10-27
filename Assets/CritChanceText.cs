using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CritChanceText : MonoBehaviour
{
    TMP_Text text;

    private void Start()
    {
        text = gameObject.GetComponent<TMP_Text>();
    }
    void Update()
    {
        text.text = $"{Mathf.Min((float)PlayerScript.instance.comboCount,50f) / 2f}%";
    }
}
