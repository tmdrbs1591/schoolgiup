using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class typingeffect : MonoBehaviour
{
    public TMP_Text tx;
    private string m_text = "니네 보물은 내가 가져간다~ㅋㅋ";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(_typing());
        Destroy(tx,4f);
       
    }
    IEnumerator _typing()
    {
       
        yield return new WaitForSeconds(0.7f);
        
        for (int i = 0; i <= m_text.Length; i++)
        {
            tx.text = m_text.Substring(0, i);
            yield return new WaitForSeconds(0.1f);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
