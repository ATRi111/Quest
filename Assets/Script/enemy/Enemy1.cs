using UnityEngine;

public class Enemy1 : CEnemy
{
    protected override void Start()
    {
        base.Start();
        NAME = "�粼��";
        HP = 16;
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
