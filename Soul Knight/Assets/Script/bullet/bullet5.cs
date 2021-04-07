using UnityEngine;
public class bullet5 : bullet
{
    
    void Start()
    {
        penetrating = true;
        damage = 3;
        criticalDamage = 8;
    }

    public void DamageUp(float rate)
    {
        damage = (int)(rate * damage);
        criticalDamage = (int)(rate * criticalDamage);
    }
}
