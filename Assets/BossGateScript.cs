using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGateScript : MonoBehaviour
{
    [SerializeField] GameObject enableObject;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.transform.position += Vector3.right * 4;
            StartCoroutine("Close");
        }
    }

    IEnumerator Close()
    {
        enableObject.SetActive(true);
        float velocity = 0;
        while (enableObject.transform.localPosition.y > 0)
        {
            enableObject.transform.localPosition -= Vector3.up * velocity;
            velocity += Time.deltaTime / 15;
            yield return null;
        }
        PlayerScript.instance.camShake += 0.4f;
        enableObject.transform.localPosition = Vector3.zero;
        Destroy(this);
        yield break;
    }
}
