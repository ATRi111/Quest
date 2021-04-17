public class Bullet2 : CBullet
{
    void Start()
    {
        b_Penetrating = true;
        Damage = 10;
        CriticalDamage = 30;
    }
}
