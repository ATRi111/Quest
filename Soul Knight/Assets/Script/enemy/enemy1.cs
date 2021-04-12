using UnityEngine;

public class enemy1 : CEnemy
{
    protected override void Start()
    {
        base.Start();
        NAME = "¸ç²¼ÁÖ";
        HP = 16;
    }

    protected override void DoAct()
    {
        base.DoAct();
        if (t_Action > 1f&&!b_HasAttacked&&b_Alive)
        {
            b_HasAttacked = true;
            GenerateDanmaku();
        }
    }
    protected override void GenerateDanmaku()
    {
        base.GenerateDanmaku();
        m_Angle += 15f;
        base.GenerateDanmaku();
        m_Angle -= 30f;
        base.GenerateDanmaku();
    }
}
