using UnityEngine;

public class enemy3 : CEnemy
{
    protected override void Start()
    {
        base.Start();
        NAME = "大型哥布林";
        HP = 40;
        UpSpeed = 3f;
        Damage = 2;
        m_Energypoint = Random.value < 0.75f ? 1 : 0;
        m_Coin = Random.value < 0.75f ? 1 : 0;
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
        m_Angle += 15f;
        base.GenerateDanmaku();
        m_Angle -= 45f;
        base.GenerateDanmaku();
        m_Angle -= 15f;
        base.GenerateDanmaku();
    }
}
