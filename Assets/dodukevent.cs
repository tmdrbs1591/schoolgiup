using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dodukevent : MonoBehaviour
{

    public GameObject tpptc;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Tpptc());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Tpptc()
    {
        yield return new WaitForSeconds(4f);
        AudioScript.instance.PlaySound(transform.position, 15, Random.Range(0.8f, 1.0f), 1);
        Destroy(Instantiate(tpptc, transform.position, Quaternion.identity),2f);
        Destroy(gameObject);
    }
}
