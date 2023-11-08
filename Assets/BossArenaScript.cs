using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArenaScript : MonoBehaviour
{
    [SerializeField] GameObject bossGate;
    bool closing;

    void Update()
    {
        if (0 == gameObject.GetComponentsInChildren<EnemyBase>().Length && !closing)
        {
            StartCoroutine(Close());
        }
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
