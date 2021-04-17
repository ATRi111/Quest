    using UnityEngine;
public class Bullet3 : CBullet
{
    void Start()
    {
        Damage = 4;
        CriticalDamage = 8;
    }
    public void DamageUp(float rate)
    {
        Damage = (int)(rate * Damage);
        CriticalDamage = (int)(rate * CriticalDamage);
    }
}
