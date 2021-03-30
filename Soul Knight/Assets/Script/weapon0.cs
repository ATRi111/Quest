using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon0 : weapon
{
    GameObject tempBullet;//下一发子弹
    Rigidbody2D tempRb;//下一发子弹的刚体

    void Start()
    {
        Name = "破旧的手枪";
        color = Color.white;
        shoot_cd = 25;
        bulletOffsetDistance = 0.5f;
        cost = 0;
    }
    protected override void GenerateBullet() 
    {
        //如果是单发子弹，不需要再计算direction和bulletoffset
        tempBullet = GameObject.Instantiate(bullet, transform.position, rotation);
        tempRb = tempBullet.GetComponent<Rigidbody2D>();
        tempRb.velocity = direction*10;
    }
}
