using UnityEngine;
public class bullet5 : CBullet
{
    void Start()
    {
        b_Penetrating = true;
        Damage = 3;
        CriticalDamage = 8;
    }
    public void DamageUp(float rate)
    {
        Damage = (int)(rate * Damage);
        CriticalDamage = (int)(rate * CriticalDamage);
    }
}
