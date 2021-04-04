using UnityEngine;

public class enemy3 : enemy
{
    protected override void Start()
    {
        base.Start();
        Name = "���͸粼��";
        HP = 40;
        speed_up = 3f;
        damage = 2;
        num_energypoint = Random.value < 0.75f ? 1 : 0;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.SendMessage("GetDamage", damage);
    }

    protected override void DoAct()
    {
        base.DoAct();
        if (count_action > 1f&&!hasAttacked&&alive)
        {
            hasAttacked = true;
            GenerateDanmaku();
        }
    }
    protected override void GenerateDanmaku()
    {
        base.GenerateDanmaku();
        angle += 15f;
        base.GenerateDanmaku();
        angle += 15f;
        base.GenerateDanmaku();
        angle -= 45f;
        base.GenerateDanmaku();
        angle -= 15f;
        base.GenerateDanmaku();
    }
}
