using UnityEngine;

public class enemy0 : enemy
{
    protected override void Start()
    {
        base.Start();
        Name = "野猪";
        speed= 3f;
        HP = 10;
        damage = 2;
        sight = 0f;//不会主动攻击
    }
}
