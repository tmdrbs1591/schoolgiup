using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public InputActionAsset input;
    public bool ready;
    public static GameManager instance;
    public int doorsBroken;
    public int doorsBrokenTotal;
    public int score;
    public int comboAdditionalScore;
    public int coin;
    public GameObject shopPenel;
    public bool shopping;
    public int boss;
    public int randomArenaLeft;
    public GameObject SettingPenel;
    public GameObject MousePtc;
    public GameObject Level1;
    public GameObject Level2;
    public GameObject ModPenel;
    public GameObject Btnptc;
    public Transform modebtn;
    public Transform setbtn;
    public bool gameOver;
    public bool inScary;
    public Sprite[] weaponSprites;
    Color bgColor;
    public int maxCombo;

    void Start()
    {
        instance = this;
        randomArenaLeft = Random.Range(2, 4);
        bgColor = Camera.main.backgroundColor;
        Invoke("Ready", 0.1f);
    }

    void Ready()
    {
        ready = true;
    }

    public void AddScore(int _score, bool comboAffected = false)
    {
        score += _score + (comboAffected ? (PlayerScript.instance.comboCount - 1) * 10 : 0 );
        comboAdditionalScore = (comboAffected ? (PlayerScript.instance.comboCount - 1) * 10 : 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShop();
        }
        if (Input.GetMouseButtonDown(0))
        {
           
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

           
            Destroy(Instantiate(MousePtc, mousePosition, Quaternion.identity),1f);
        }
        if (SceneManager.GetActiveScene().name == "Title") return;
        Camera.main.backgroundColor = inScary ? Color.black:bgColor;
        PlayerScript.instance.lighting.SetActive(inScary);
    }

    public void OpenShop()
    {
        if (shopping) return;
        shopPenel.SetActive(true);
        shopping = true;
        AudioScript.instance.PlaySound(PlayerScript.instance.transform.position, 10);   
    }
    public void ModPenelOpen()
    {
        ModPenel.SetActive(true);
        Destroy(Instantiate(Btnptc, modebtn.transform.position, Quaternion.identity), 1.5f);
        AudioScript.instance.PlaySound(transform.position, 19, Random.Range(0.8f, 1.0f), 1);
        AudioScript.instance.PlaySound(transform.position, 18, Random.Range(0.8f, 1.0f), 1);

    }
    public void ModPenelClose()
    {
        ModPenel.SetActive(false);
    }
   public void CloseShop()
    {
        shopPenel?.SetActive(false);
        shopping = false;
    }
    public void OpenSetting()
    {
        SettingPenel.SetActive(true);
        Destroy(Instantiate(Btnptc, setbtn.transform.position, Quaternion.identity),1.5f);
        AudioScript.instance.PlaySound(transform.position, 19, Random.Range(0.8f, 1.0f), 1);
        AudioScript.instance.PlaySound(transform.position, 18, Random.Range(0.8f, 1.0f), 1);
    }
    public void CloseSetting()
    {
        SettingPenel.SetActive(false);
    }
    public void nextLevel()
    {
        Level1.SetActive(false);    
        Level2.SetActive(true);
    }
    public void previousLevel()
    {
        Level1.SetActive(true);
        Level2.SetActive(false);
    }

}
