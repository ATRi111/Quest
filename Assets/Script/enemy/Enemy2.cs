using Public;
using System.Collections;
using UnityEngine;

public class Enemy2 : CEnemy
{
    protected override void Start()
    {
        base.Start();
        NAME = "花";
        HP = 12;
        T_ATTACK = 2f;
        T_IDLE = 2f;
    }
    protected override IEnumerator Idle()
    {
        m_Rigidbody.velocity = Vector3.zero;
        yield return CTool.Wait(T_IDLE);
        StartCoroutine(nameof(Attack));  //只有攻击、静止两个阶段
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
