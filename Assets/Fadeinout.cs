using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fadeinout : MonoBehaviour
{
    [SerializeField]
    private Slider Fadein;
   

    private float decreaseAmount = 2f;

    void Update()
    {
       
            FadeinStart();



    }

    public void FadeinStart()
    {
       
        gameObject.SetActive(true);

        float currentValue = Fadein.value;
        currentValue += decreaseAmount * Time.deltaTime;


        Fadein.value = currentValue;


        if (currentValue <= 1)
        {
            Invoke("LoadScene", 0.52f);
        }

    }
    public void LoadScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

   
}
