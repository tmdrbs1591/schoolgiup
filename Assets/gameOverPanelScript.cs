using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameOverPanelScript : MonoBehaviour
{
    [SerializeField] RectTransform rect;
    float gameOverTimer;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.gameOver)
            gameOverTimer += Time.unscaledDeltaTime;
        
        if (gameOverTimer >= 3)
            rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition,Vector2.zero,Time.unscaledDeltaTime * 3);
        
        
    }
}
