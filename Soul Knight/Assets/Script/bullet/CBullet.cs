using UnityEngine;
using Public;
//�����ӵ��ĸ���
public class CBullet : MonoBehaviour
{
    protected int Damage { set; get; }
    protected int CriticalDamage { set; get; }    //�����˺�
    protected bool b_Penetrating=false;           //�д�͸�Ե�
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
