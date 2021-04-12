using UnityEngine;
using static CAudioController;

public class weapon2 : CWeapon
{
    protected override void Start()
    {
        base.Start();
        INFO = "¼«¹â";
        SHOOT_CD = 1000;
        COST = 4;
        m_DeflectLevel = 0f;
        SHOOT_SPEED = 40;
        fx_Weapon = "fx_weapon2";
    }
}
