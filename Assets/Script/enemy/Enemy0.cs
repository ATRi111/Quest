using UnityEngine;

public class Enemy0 : CEnemy
{
    protected override void Start()
    {
        base.Start();
        NAME = "Ұ��";
        Speed= 3f;
        HP = 10;
        m_Damage = 2;
        VISION = 0f;//������������
    }
}
