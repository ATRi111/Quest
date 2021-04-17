using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class danmaku2 : danmaku
{
    Rigidbody2D rb;
    void Start()
    {
        damage = 3;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity *= Random.Range(0.5f,1f);
    }
    void FixedUpdate()
    {
        rb.velocity -=rb.velocity.normalized*0.04f;
        if (rb.velocity.magnitude < 1f) Destroy(this.gameObject);
    }
}
