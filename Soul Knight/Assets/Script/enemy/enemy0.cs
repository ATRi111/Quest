using UnityEngine;

public class enemy0 : enemy
{
    protected override void Start()
    {
        base.Start();
        Name = "Ұ��";
        speed_up = 4f;
        HP = 10;
        damage = 2;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("GetDamage", damage);
            actionIng = false;//�������һ�κ�����ж�
        }   
    }

}
