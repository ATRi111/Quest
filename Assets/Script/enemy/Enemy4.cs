using UnityEngine;

public class Enemy4 : CEnemy
{
    protected override void Start()
    {
        base.Start();
        NAME = "¥Û“∞÷Ì";
        Speed = 3f;
        UpSpeed = 4f;
        HP = 20;
        m_Damage = 3;
        VISION = 6f;
    }
}
