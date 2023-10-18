using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTextScript : MonoBehaviour
{
    int oldScore = 0;
    TMP_Text scoreText;
    [SerializeField] TMP_Text addScoreText;
    [SerializeField] TMP_Text addComboScoreText;
    RectTransform transforma;
    float randomAffector;
    float addTextTimer;
    float addComboTextTimer;

    private void Start()
    {
        scoreText = GetComponent<TMP_Text>();
        transforma = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (oldScore != GameManager.instance.score)
        {
            addScoreText.text = $"+{GameManager.instance.score - oldScore - GameManager.instance.comboAdditionalScore}";
            oldScore = GameManager.instance.score;
            scoreText.text = $"{oldScore}";
            randomAffector = 0.5f;
            addTextTimer = 2;
            if (GameManager.instance.comboAdditionalScore > 0)
            {
                addComboScoreText.text = $"+{GameManager.instance.comboAdditionalScore}";
                addComboTextTimer = 2;
            }
        }
        addScoreText.color = new Color(0, 1, 0, addTextTimer);
        addComboScoreText.color = new Color(1, 1, 0, addComboTextTimer);
        transforma.anchoredPosition = new Vector2(Random.Range(-randomAffector * 20, randomAffector * 20), -151 + Random.Range(-randomAffector * 20, randomAffector * 20));
        if (randomAffector > 0) randomAffector -= Time.deltaTime;
        if (addTextTimer > 0) addTextTimer -= Time.deltaTime;
        if (addComboTextTimer > 0) addComboTextTimer -= Time.deltaTime;
    }
}
