using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextAlpha : MonoBehaviour
{

    public TextMeshProUGUI text;
    float time = 0f;
    float F_time = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeFlow());
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    IEnumerator FadeFlow()
    {
        Color Alpha = text.color;
        while (Alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            Alpha.a = Mathf.Lerp(0.1f,1f,time);
            text.color = Alpha;
            yield return null;
            }
        time = 0f;
        yield return new WaitForSeconds(1f);

        while (Alpha.a < 0f)
        {
            time += Time.deltaTime / F_time;
            Alpha.a = Mathf.Lerp(0.1f,1f, time);
            text.color = Alpha;
            yield return null;
        }

    }

   
}
