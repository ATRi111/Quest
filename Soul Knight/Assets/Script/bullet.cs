using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//所有子弹的父类
public class bullet : MonoBehaviour
{
    public int damage=0;
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if(GameObject.Find("knight").GetComponent<player>().IsCritical()) damage*=2;
        }
    }
    protected void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Wall": 
            case "Door": 
            case "Obstacle": 
            case "Enemy": Destroy(this.gameObject);break;
        }    
    }
}
