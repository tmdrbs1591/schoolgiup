using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemyScript : EnemyBase
{
    void FixedUpdate()
    {
        rigid.velocity = new Vector2(rigid.velocity.x / 1.1f,rigid.velocity.y);
    }
}
