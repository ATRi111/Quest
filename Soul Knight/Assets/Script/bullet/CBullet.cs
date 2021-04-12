using UnityEngine;
using Public;
//所有子弹的父类
public class CBullet : MonoBehaviour
{
    protected int Damage { set; get; }
    protected int CriticalDamage { set; get; }    //暴击伤害
    protected bool b_Penetrating=false;           //有穿透性的
    protected float DamageRate = 1.0001f;
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        IDamagable obj1 = collision.gameObject.GetComponent<IDamagable>();
        if (obj1 != null)
        {
            obj1.GetDamage(Damage);
            if (!b_Penetrating) Destroy(this.gameObject);
        }
        else 
            Destroy(this.gameObject);
    }
    public void SetDamageRate(float rate) => DamageRate = rate;
}
