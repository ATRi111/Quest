using Public;
using UnityEngine;
using static CSceneManager;
using static CAudioController;
 
public class CBox : MonoBehaviour, IDamagable
{
    public int HP { set; get; } = 4;
    bool m_EnergyPoint;//将会掉落一个能量点

    void Start()
    {
        m_EnergyPoint = Random.value < 0.25f;
    }

    public void GetDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0) Die();
    }

    public void Die()
    {
        if (m_EnergyPoint) GenerateEnergyPoint(transform.position);
        CAudioController.PlayAudio("fx_broke");
        Destroy(this.gameObject);
    }
}
