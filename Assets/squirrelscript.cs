using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class squirrelscript : MonoBehaviour
{
    [SerializeField]
    GameObject coin;
    [SerializeField]
    GameObject clearptc;
    SpriteRenderer sprite;
    [SerializeField]
    Sprite smlie;
    [SerializeField] GameObject acomimage;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && PlayerScript.instance.GetAcom)
        {
            AudioScript.instance.PlaySound(transform.position, 22, 1, 1);
            Destroy(Instantiate(clearptc, transform.position, Quaternion.identity), 2f);
            PlayerScript.instance.GetAcom = false;
            acomimage.SetActive(false);


            if (coin != null)
            {
                sprite.sprite = smlie;
                for (int i = 0; i < 10; i++)
                {
                    Instantiate(coin, transform.position, Quaternion.identity);
                }
            }
        }
    }
}
