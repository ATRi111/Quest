using UnityEngine;

public class Enemy4 : CEnemy
{
    protected override void Start()
    {
        base.Start();
        NAME = "��Ұ��";
        Speed = 3f;
        UpSpeed = 4f;
        HP = 20;
        m_Damage = 3;
        VISION = 6f;
    }
}
