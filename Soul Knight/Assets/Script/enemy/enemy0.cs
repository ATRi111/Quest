using UnityEngine;

public class enemy0 : CEnemy
{
    protected override void Start()
    {
        base.Start();
        NAME = "Ұ��";
        Speed= 3f;
        HP = 10;
        Damage = 2;
        VISION = 0f;//������������
    }
}
