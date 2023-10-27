using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangScript : MonoBehaviour
{
    Rigidbody2D rigid;
    float speed = 40;
    Vector3 velocity;


    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        velocity = new Vector3(PlayerScript.instance.dir, 0, 0);

        Transform tMIn = null;
        float minDist = 10;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in GameObject.FindGameObjectsWithTag("Enemy"))
        {

            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMIn = t.transform;
                minDist = dist;
            }
        }

        if (tMIn != null) { velocity = Vector3.ClampMagnitude((tMIn.position - transform.position) * 5, 1); }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.fwd * 720 * Time.deltaTime);
        transform.position += velocity * Time.deltaTime * speed;
        if (speed < 0)
        {
            velocity = -Vector3.ClampMagnitude((PlayerScript.instance.transform.position - transform.position) * 3, 1);
            if (Vector3.Distance(transform.position, PlayerScript.instance.transform.position) < 1)
               Destroy(gameObject);
        }
        speed -= Time.deltaTime * 80;
    }
}
