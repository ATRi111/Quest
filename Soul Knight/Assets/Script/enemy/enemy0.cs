using UnityEngine;

public class enemy0 : enemy
{
    protected override void Start()
    {
        base.Start();
        Name = "Ұ��";
        speed = 3f;
        speed_up = 5f;
        HP = 10;
        damage = 3;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Player")) actionIng = false;//ײ��һ�κ��ֹͣ
    }
    protected override void DoAct()
    {
        base.DoAct();
    }
}
