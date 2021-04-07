using UnityEngine;

public class enemy2 : enemy
{

    protected override void Start()
    {
        base.Start();
        Name = "花";
        HP = 12;
        speed = speed_up = 0;//无法移动
        cd_action = 4f;
    }
    protected override void Act()
    {
        if (actionReady)//总是处于战斗状态
        {
            hasAttacked = false;
            actionIng = true;
            actionReady = false;
            Invoke(nameof(EndAction), t_action);
            Invoke(nameof(ResetAction), cd_action);
        }
        else if (actionIng)
        {
            DoAct();
            count_action += Time.fixedDeltaTime;
        }
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
