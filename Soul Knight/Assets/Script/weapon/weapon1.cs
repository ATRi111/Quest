using UnityEngine;
using static CAudioController;

public class weapon1 : CWeapon
{
    protected override void Start()
    {
        base.Start();
        INFO = "ö±µ¯³å·æÇ¹";
        SHOOT_CD = 500;
        COST = 2;
        m_DeflectLevel = 10f;
        fx_Weapon = "fx_weapon1";
    }
    protected override void GenerateBullet()
    {
        base.GenerateBullet();
        angle += 10f;
        base.GenerateBullet();
        angle -= 20f; 
        base.GenerateBullet();
    }
}
