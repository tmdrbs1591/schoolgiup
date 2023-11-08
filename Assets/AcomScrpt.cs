using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcomScrpt : MonoBehaviour
{
    [SerializeField] GameObject getptc;
    [SerializeField] GameObject acomimage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            
            PlayerScript.instance.GetAcom = true;
            acomimage.SetActive(true);
            Destroy(Instantiate(getptc, transform.position, Quaternion.identity), 3f);
            Destroy(gameObject);
        }
    }
}
