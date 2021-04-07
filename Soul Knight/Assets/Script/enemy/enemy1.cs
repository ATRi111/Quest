using UnityEngine;

public class enemy1 : enemy
{
    protected override void Start()
    {
        base.Start();
        Name = "¸ç²¼ÁÖ";
        HP = 16;
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
        angle -= 30f;
        base.GenerateDanmaku();
    }
}
