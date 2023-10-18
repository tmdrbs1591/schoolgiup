using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    Rigidbody2D rigidBody;
    TrailRenderer trail;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
        Jump();
    }

    void Jump()
    {
        float randomJumpForce = Random.Range(4f, 6f);
        Vector2 jumpVelocity = new Vector2(Random.Range(-1f, 1f), randomJumpForce);
        rigidBody.AddForce(jumpVelocity, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (rigidBody.velocity.magnitude <= 0.01f && rigidBody.simulated)
        {
            rigidBody.simulated = false;
            StartCoroutine(GoToPlayer());
        }
    }

    IEnumerator GoToPlayer()
    {
        Vector3 startPos = transform.position;
        trail.emitting = true;
        yield return new WaitForSeconds(0.2f);
        for (float i = 0; i < 1; i += Time.deltaTime * 2)
        {
            transform.position = Vector3.Lerp(startPos,PlayerScript.instance.transform.position,((i*10)* (i * 10)) / 100);
            yield return null;
        }
        GameManager.instance.coin++;
        AudioScript.instance.PlaySound(transform.position, 8, Random.Range(0.9f, 1.1f), 1);
        Destroy(gameObject);
        yield return null;
    }
}
