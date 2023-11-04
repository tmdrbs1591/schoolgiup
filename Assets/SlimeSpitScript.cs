using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeSpitScript : MonoBehaviour
{
    [SerializeField]
    GameObject particle;
    
    public void Explode() {
        Destroy(Instantiate(particle,transform.position,Quaternion.identity),1);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
            Explode();
    }
}
