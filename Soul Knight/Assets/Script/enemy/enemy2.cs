using UnityEngine;

public class enemy2 : enemy
{

    protected override void Start()
    {
        base.Start();
        Name = "��";
        HP = 12;
        speed = speed_up = 0;//�޷��ƶ�
        cd_action = 4f;
    }
    protected override void Act()
    {
        if (actionReady)//���Ǵ���ս��״̬
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
