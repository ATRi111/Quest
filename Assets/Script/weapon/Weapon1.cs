using UnityEngine;
using static CAudioController;

public class Weapon1 : CWeapon
{
    protected override void Start()
    {
        base.Start();
        INFO = "�������ǹ";
        SHOOT_CD = 500;
        COST = 2;
        m_DeflectLevel = 10f;
        fx_Weapon = "fx_weapon1";
    }
    protected override void GenerateBullet()
    {
        base.GenerateBullet();
        m_Angle += 10f;
        base.GenerateBullet();
        m_Angle -= 20f; 
        base.GenerateBullet();
    }
}
