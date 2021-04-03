using UnityEngine;

public class enemy0 : enemy
{
    protected override void Start()
    {
        base.Start();
        Name = "野猪";
        speed_up = 4f;
        HP = 10;
        damage = 2;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("GetDamage", damage);
            actionIng = false;//攻击玩家一次后结束行动
        }   
    }

}
