using UnityEngine;

public class enemy0 : enemy
{
    protected override void Start()
    {
        base.Start();
        Name = "野猪";
        speed = 2f;
        speed_up = 4f;
        HP = 10;
        damage = 2;
    }
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.CompareTag("Player")) actionIng = false;//攻击玩家一次后结束行动
    }
    protected override void DoAct()
    {
        base.DoAct();
    }
}
