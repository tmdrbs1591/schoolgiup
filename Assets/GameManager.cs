using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int doorsBroken;
    public int score;
    public int comboAdditionalScore;
    public int coin;

    void Start()
    {
        instance = this;
    }

    public void AddScore(int _score, bool comboAffected = false)
    {
        score += _score + (comboAffected ? (PlayerScript.instance.comboCount - 1) * 10 : 0 );
        comboAdditionalScore = (comboAffected ? (PlayerScript.instance.comboCount - 1) * 10 : 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
