using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon0 : weapon
{
    GameObject tempBullet;//��һ���ӵ�
    Rigidbody2D tempRb;//��һ���ӵ��ĸ���

    void Start()
    {
        Name = "�ƾɵ���ǹ";
        color = Color.white;
        shoot_cd = 25;
        bulletOffsetDistance = 0.5f;
        cost = 0;
    }
    protected override void GenerateBullet() 
    {
        //����ǵ����ӵ�������Ҫ�ټ���direction��bulletoffset
        tempBullet = GameObject.Instantiate(bullet, transform.position, rotation);
        tempRb = tempBullet.GetComponent<Rigidbody2D>();
        tempRb.velocity = direction*10;
    }
}
