using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComboMeterScript : MonoBehaviour
{
    TMP_Text counter;
    Slider meter;

    void Start() {
        counter = gameObject.GetComponentInChildren<TMP_Text>();
        meter = gameObject.GetComponent<Slider>();
    }

    void Update()
    {
        meter.value = PlayerScript.instance.comboTime;
        counter.text = $"x{PlayerScript.instance.comboCount}";
    }
}
