using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBtnScript : MonoBehaviour
{

    public GameObject StartBtnPtc;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ptc()
    {
        Instantiate(StartBtnPtc,transform.position,Quaternion.identity);
        AudioScript.instance.PlaySound(transform.position, 17, Random.Range(0.8f, 1.0f), 1);
    }
}
