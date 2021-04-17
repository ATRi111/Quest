using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//所有弹幕的父类
public class danmaku : MonoBehaviour
{
    protected int damage;
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Wall": case "Door": case "Obstacle": Destroy(this.gameObject); break;
            case "Player":case "Box":
                collision.gameObject.SendMessage("GetDamage", damage);
                Destroy(this.gameObject); break;
        }
    }
}
