using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComboMeterScript : MonoBehaviour
{
    TMP_Text counter;
    Slider meter;
    RectTransform rectT;
    RectTransform rectText;
    int oldCombo;
    float counterShake;

    void Start() {
        rectT = gameObject.GetComponent<RectTransform>();
        counter = gameObject.GetComponentInChildren<TMP_Text>();
        meter = gameObject.GetComponent<Slider>();
        rectText = counter.gameObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        rectT.anchoredPosition = Vector2.Lerp(rectT.anchoredPosition,new Vector2(rectT.anchoredPosition.x,PlayerScript.instance.comboCount > 0 ? -200:300),Time.deltaTime * 10);
        meter.value = PlayerScript.instance.comboTime;
        if (oldCombo != PlayerScript.instance.comboCount) {
            oldCombo = PlayerScript.instance.comboCount;
            counter.text = $"x{oldCombo}";
            counterShake = 0.5f;
            counter.fontSize += 20;
        }
        rectText.anchoredPosition = new Vector2(Random.Range(-counterShake * 10,counterShake * 10),-46 + Random.Range(-counterShake * 10,counterShake * 10));
        counter.fontSize = Mathf.Lerp(counter.fontSize,90,Time.deltaTime * 10);
        if (counterShake >0) counterShake -= Time.deltaTime;
    }
}
