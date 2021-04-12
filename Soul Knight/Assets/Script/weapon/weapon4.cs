using UnityEngine;
using static CAudioController;

public class weapon4 : CWeapon
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
        extraOffset = CSceneManager.Angle2Direction(angle + 90f) * 0.5f;
        base.GenerateBullet();
        base.GenerateBullet();
        TempBullet.transform.position += extraOffset;
        base.GenerateBullet();
        TempBullet.transform.position -= extraOffset;
    }
}
