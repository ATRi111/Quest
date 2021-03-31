using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy0 : enemy
{
    protected override void Start()
    {
        base.Start();
        speed = 4f;speed_up = 6f;
        HP = 10;
    }
    protected override void DoAct()
    {
        
    }
}
