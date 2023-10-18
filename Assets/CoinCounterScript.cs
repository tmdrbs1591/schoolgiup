using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCounterScript : MonoBehaviour
{
    int oldCoins = 0;
    TMP_Text scoreText;
    RectTransform transforma;
    float randomAffector;

    private void Start()
    {
        scoreText = GetComponent<TMP_Text>();
        transforma = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (oldCoins != GameManager.instance.coin)
        {
            oldCoins = GameManager.instance.coin;
            scoreText.text = $"{oldCoins}";
            randomAffector = 0.5f;
        }
        transforma.anchoredPosition = new Vector2(122 +Random.Range(-randomAffector * 10, randomAffector * 10), Random.Range(-randomAffector * 10, randomAffector * 10));
        if (randomAffector > 0) randomAffector -= Time.deltaTime;
    }
}
