using UnityEngine;
using static CAudioController;

public class weapon5 : CBow
{
    protected override void Start()
    {
        base.Start();
        INFO = "风之弓";
        SHOOT_CD = 500;
        COST = 2;
        m_DeflectLevel = 0f;
        m_BulletOffsetDistance = 0f;
        SHOOT_SPEED = 30;
        TIGHTNESS_START = 0.5f;
        fx_Weapon = "fx_release";
        fx_Tighten ="fx_weapon5";
    }

    protected override void Release()
    {
        base.Release();
        Player.GetComponent<CPlayer>().SpeedUp(1f); //射击后使角色加速
    }
}
