using UnityEngine;

public class enemy0 : enemy
{
    protected override void Start()
    {
        base.Start();
        Name = "Ұ��";
        speed= 3f;
        HP = 10;
        damage = 2;
        sight = 0f;//������������
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.SendMessage("GetDamage", damage); 
    }
}
