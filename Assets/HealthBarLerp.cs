using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarLerp : MonoBehaviour
{
    Slider s;
    Slider Ss;
    void Start()
    {
        s = transform.parent.GetComponent <Slider>();
        Ss = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        Ss.maxValue = s.maxValue;
        Ss.value = Mathf.Lerp(Ss.value, s.value, Time.deltaTime*4f);
    }
}
