using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageCountScript : MonoBehaviour
{
    TMP_Text counter;
    int oldStage;

    void Start() {
        counter = gameObject.GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (oldStage != GameManager.instance.doorsBrokenTotal) {
            oldStage = GameManager.instance.doorsBrokenTotal;
            counter.text = $"{oldStage}";
            counter.fontSize += 20;
        }
        counter.fontSize = Mathf.Lerp(counter.fontSize,75,Time.deltaTime * 10);
    }
}
