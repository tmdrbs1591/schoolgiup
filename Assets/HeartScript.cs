using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartScript : MonoBehaviour
{
    Vector3 velocity;
    float dieTimer;
    bool dying;
    Image sprite;

    void Start() {
        sprite = GetComponent<Image>();
    }

    public void Die() {
        transform.SetParent(transform.parent.parent,true);
        velocity = new Vector3(Random.Range(-200,200),Random.Range(10,50),0);
        dying = true;
        sprite.color = new Color(1,1,1,0.5f);
    }

    void Update() {
        transform.localScale = Vector3.Lerp(transform.localScale,Vector3.one,Time.deltaTime * 10);
        if (!dying) return;
        transform.position += velocity * Time.deltaTime;
        velocity.y -= Time.deltaTime * 900;
        dieTimer += Time.deltaTime;
        if (dieTimer > 5) Destroy(gameObject);  
    }
}
