using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HitTextScript : MonoBehaviour
{
    TMP_Text text;
    float damage = 10;
    Vector3 velocity;
    [SerializeField] Color critColor;

    public void Initialize(float dmg, bool crit = false)
    {
        text = gameObject.GetComponent<TMP_Text>();
        damage = dmg;
        float scaleFactor = Mathf.Min(0.8f + dmg / 10,1.5f);
        transform.localScale *= scaleFactor;
        velocity = new Vector3(Random.Range(-2,2), 7, 0);
        text.text = $"{dmg}";
        if (crit) { text.color = critColor; }
    }

    // Update is called once per frame
    void Update()
    {
        velocity.y -= Time.deltaTime * 18;
        transform.position += velocity * Time.deltaTime;
        if (velocity.y < 0)
        {
            text.color -= new Color(0, 0, 0, Time.deltaTime * 5);
            if (text.color.a <= 0) Destroy(gameObject);
        }
    } 
}
