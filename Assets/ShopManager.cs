using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    public GameObject[] weaponcards;
    public Transform[] cardHolder;
    public static ShopManager instance;
    [SerializeField]
    int price = 10;
    float rotAccel = 0;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        Reroll();
    }

    public void Reroll()
    {
        int[] made = new int[4];
        for (int i = 0; i < 4; i++)
        {
            if (cardHolder[i].childCount > 0) {
                foreach (Transform a in cardHolder[i])
                {
                    Destroy(a.gameObject);
                }
            }
            int rand = Random.Range(0, weaponcards.Length);
            while (made.Contains(rand))
            {
                rand = Random.Range(0, weaponcards.Length);
            }
            made[i] = rand;
            Instantiate(weaponcards[rand], cardHolder[i]);
        }
    }

    private void Update()
    {
        foreach (Transform a in cardHolder)
        {
            a.eulerAngles = Vector3.up * rotAccel;
        }
        if (rotAccel > 0) rotAccel = Mathf.Lerp(rotAccel, 0, 30 * Time.deltaTime);
    }

    public void Rerollbuy()
    {
        if (GameManager.instance.coin >= price)
            {
                AudioScript.instance.PlaySound(PlayerScript.instance.transform.position, 9);
                GameManager.instance.coin -= price;
                rotAccel = 900;
                Reroll();
            }
        
    }
}
