using UnityEngine;
using Public;
using static CAudioController;

public class Weapon4 : CWeapon
{
    public Vector3 extraOffset;
    protected override void Start()
    {
        base.Start();
        INFO = "Ñ©ºü ÍÁºÀ½ð";
        SHOOT_CD = 250;
        COST = 3;
        m_DeflectLevel = 0f;
        fx_Weapon = "fx_weapon4";
    }
    protected override void GenerateBullet()
    {
        extraOffset = CTool.Angle2Direction(m_Angle + 90f) * 0.5f;
        base.GenerateBullet();
        base.GenerateBullet();
        TempBullet.transform.position += extraOffset;
        base.GenerateBullet();
        TempBullet.transform.position -= extraOffset;
    }
}
