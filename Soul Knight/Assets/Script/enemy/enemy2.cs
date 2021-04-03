using UnityEngine;

public class enemy2 : enemy
{

    protected override void Start()
    {
        base.Start();
        Name = "»¨";
        HP = 10;
        speed = speed_up = 0;
    }
    protected override void DoAct()
    {
        base.DoAct();
        if (count_action > 1f&&!hasAttacked&&alive)
        {
            hasAttacked = true;
            GenerateDanmaku();
        }
    }
    protected override void GenerateDanmaku()
    {
        for(int i=0;i<5;i++)
        {
            angle = Random.Range(0f, 360f);
            base.GenerateDanmaku();
        }
    }
}
