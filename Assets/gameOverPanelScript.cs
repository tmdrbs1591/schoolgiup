using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class gameOverPanelScript : MonoBehaviour
{
    [SerializeField] RectTransform rect;
    float gameOverTimer;
    [SerializeField] TMP_Text resultText;
    bool gameOvered;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameOver && !gameOvered)
            StartCoroutine(die());
    }

    IEnumerator die() {
        gameOvered = true;
        yield return new WaitForSecondsRealtime(3);
        resultText.text = $"0\n0\n0";
        float startY = rect.anchoredPosition.y;
        for (float i = 0; i < 1; i += Time.unscaledDeltaTime) {
            rect.anchoredPosition = Vector2.Lerp(Vector2.up * startY,Vector2.zero,i);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(0.7f);

        for (int scoreCount = 0; scoreCount < GameManager.instance.score; scoreCount += 2000) {
            resultText.text = $"{scoreCount}\n{0}\n{0}";
            AudioScript.instance.PlaySound(transform.position,27, 1 + scoreCount / 20000f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        resultText.text = $"{GameManager.instance.score}\n{0}\n{0}";
        AudioScript.instance.PlaySound(transform.position,29);

        yield return new WaitForSecondsRealtime(0.5f);

        for (int comboCount = 0; comboCount < GameManager.instance.maxCombo; comboCount++) {
            resultText.text = $"{GameManager.instance.score}\n{comboCount}\n{0}";
            AudioScript.instance.PlaySound(transform.position,27, 1 + comboCount / 50f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        resultText.text = $"{GameManager.instance.score}\n{GameManager.instance.maxCombo}\n{0}";
        AudioScript.instance.PlaySound(transform.position,29);
        
        yield return new WaitForSecondsRealtime(0.5f);

        for (int stageCount = 0; stageCount < GameManager.instance.doorsBrokenTotal; stageCount++) {
            resultText.text = $"{GameManager.instance.score}\n{GameManager.instance.maxCombo}\n{stageCount}";
            AudioScript.instance.PlaySound(transform.position,27, 1 + stageCount / 10f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
        resultText.text = $"{GameManager.instance.score}\n{GameManager.instance.maxCombo}\n{GameManager.instance.doorsBrokenTotal}";
        AudioScript.instance.PlaySound(transform.position,28);
    }
}
