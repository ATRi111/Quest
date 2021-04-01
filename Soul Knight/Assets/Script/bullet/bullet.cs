using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//所有子弹的父类
public class bullet : MonoBehaviour
{
    public short damage=0;
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Wall": case "Door": case "Obstacle": Destroy(this.gameObject); break;
            case "Enemy":
                if (collision.CompareTag("Enemy"))
                {
                    if (GameObject.Find("knight").GetComponent<player>().TellCritical()) damage *= 2;
                    collision.gameObject.SendMessage("GetDamage", damage);
                }
                Destroy(this.gameObject); break;
            case "Box": Destroy(collision.gameObject); Destroy(this.gameObject); break;
        }    
    }
}
