using UnityEngine;

public class enemy2 : CEnemy
{

    protected override void Start()
    {
        base.Start();
        NAME = "花";
        HP = 12;
        Speed = UpSpeed = 0;//无法移动
        ACTION_CD = 4f;
    }
    protected override void Act()
    {
        if (b_ActionReady)//总是处于战斗状态
        {
            b_HasAttacked = false;
            b_ActionIng = true;
            b_ActionReady = false;
            Invoke(nameof(EndAction), ACTION_TIME);
            Invoke(nameof(ResetAction), ACTION_CD);
        }
        else if (b_ActionIng)
        {
            DoAct();
            t_Action += Time.fixedDeltaTime;
        }
    }
    protected override void DoAct()
    {
        base.DoAct();
        if (t_Action > 1f&&!b_HasAttacked&&b_Alive)
        {
            b_HasAttacked = true;
            GenerateDanmaku();
        }
    }
    protected override void GenerateDanmaku()
    {
        for(int i=0;i<5;i++)
        {
            m_Angle = Random.Range(0f, 360f);
            base.GenerateDanmaku();
        }
    }
}
