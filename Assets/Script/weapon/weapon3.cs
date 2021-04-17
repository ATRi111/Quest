using UnityEngine;
using static CAudioController;

public class weapon3 : CBow
{
    protected override void Start()
    {
        base.Start();
        INFO = "³¤¹­";
        SHOOT_CD = 1000;
        COST = 2;
        m_DeflectLevel = 0f;
        m_BulletOffsetDistance = 0f;
        SHOOT_SPEED = 20;
        TIGHTNESS_START = 0.4f;
        fx_Weapon = "fx_release";
        fx_Tighten = "fx_weapon3";
    }
}
