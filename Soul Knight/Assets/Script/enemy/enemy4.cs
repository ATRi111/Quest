using UnityEngine;

public class enemy4 : enemy
{
    protected override void Start()
    {
        base.Start();
        Name = "��Ұ��";
        speed = 3f;
        speed_up = 4f;
        HP = 20;
        damage = 3;
        sight = 6f;
    }
}
