using UnityEngine;
using static CAudioController;

public class weapon0 : CWeapon
{
    protected override void Start()
    {
        base.Start();
        INFO = "ѩ��";
        SHOOT_CD = 250;
        COST = 0;
        m_DeflectLevel = 5f;
        fx_Weapon = "fx_weapon0";
    }
}
