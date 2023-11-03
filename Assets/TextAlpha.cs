using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextAlpha : MonoBehaviour
{

    public TMP_Text text;
    [SerializeField]
    float time = 0f;
    [SerializeField]
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
        yield return new WaitForSeconds(time);
        Color Alpha = text.color;
        for (float i = 0; i < F_time; i += Time.deltaTime)
        {
            Alpha.a = Mathf.Lerp(1,0,i);
            text.color = Alpha;
            yield return null;
        }
        Destroy(gameObject);
        yield break;
    }

   
}
