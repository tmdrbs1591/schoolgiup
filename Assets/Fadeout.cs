using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fadeout : MonoBehaviour
{
    [SerializeField]
    private Slider Fadeouts;


    private float decreaseAmount = 2f;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        FadepoutStart();
    }
    public void FadepoutStart()
    {
        

        float currentValue = Fadeouts.value;
        currentValue -= decreaseAmount * Time.deltaTime;


        Fadeouts.value = currentValue;


        if (currentValue <= 0)
        {
            gameObject.SetActive(false);
        }

    }
}
