using UnityEngine;
public class bullet3 : bullet
{
    
    void Start()
    {
        damage = 2;
        criticalDamage = 4;
    }

    public void DamageUp(float rate)
    {
        damage = (int)(rate * damage);
        criticalDamage = (int)(rate * criticalDamage);
    }
}
