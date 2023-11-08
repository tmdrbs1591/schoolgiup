using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BasketBallArenaScript : MonoBehaviour
{
    [SerializeField] GameObject bossGate;
    [SerializeField] GameObject ball;
    bool closing;

    public void Goal(bool win)
    {
        Destroy(ball);
        StartCoroutine(Close());
    }

    IEnumerator Close()
    {
        float velocity = 0;
        closing = true;
        while (bossGate.transform.localPosition.y < 4)
        {
            bossGate.transform.localPosition += Vector3.up * velocity;
            velocity += Time.deltaTime / 10;
            yield return null;
        }
        PlayerScript.instance.camShake += 0.4f;
        bossGate.transform.localPosition = Vector3.up * 4;
        Destroy(this);
        yield break;
    }
}
