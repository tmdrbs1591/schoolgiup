using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class NinjaStarScript : MonoBehaviour
{
    Rigidbody2D rigid;
    float speed = 10;
    Vector3 velocity;
    bool seen;
    

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        speed = PlayerScript.instance.dir * 30;
        velocity = new Vector3(speed, 0, 0);
        Destroy(gameObject, 0.3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.fwd * 720 * Time.deltaTime);
        if (!seen)
        {

            Transform tMIn = null;
            float minDist = 7;
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
            if (tMIn != null) { velocity = Vector3.ClampMagnitude((tMIn.position - transform.position) * 500,30); seen = true; }
        }
        transform.position += velocity * Time.deltaTime;
    }
    
}
