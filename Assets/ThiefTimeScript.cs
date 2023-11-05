using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThiefTimeScript : MonoBehaviour
{
    [SerializeField]
    float time = 10;
    [SerializeField]
    EnemyBase Thief;
    [SerializeField]
    GameObject tpptc;
    TMP_Text tx;
    [SerializeField]
    Slider timeSlider;
   
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Thief.dead)
            Destroy(this);      
        time -= Time.deltaTime;
        timeSlider.value = time;
        if (time < 0)
        {

            
            GameManager.instance.coin /= 2;


            Destroy(Instantiate(tpptc,Thief.transform.position, Quaternion.identity),3f);
            AudioScript.instance.PlaySound(transform.position, 15, Random.Range(0.8f, 1.0f), 1);
            Destroy(Thief.gameObject);
            Destroy(this);

        }
    }

   
}

