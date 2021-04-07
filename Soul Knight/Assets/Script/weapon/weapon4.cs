using UnityEngine;

public class weapon4 : weapon
{
    public Vector3 extraOffset;
    protected override void Start()
    {
        base.Start();
        text = "Ñ©ºü ÍÁºÀ½ð";
        cd_shoot = 250;
        cost = 3;
        deflectLevel = 0f;
        fx_weapon = scene.FindAudio("fx_weapon4");
    }
    protected override void GenerateBullet()
    {
        extraOffset = scene.Angle2Direction(angle + 90f) * 0.5f;
        base.GenerateBullet();
        base.GenerateBullet();
        tempBullet.transform.position += extraOffset;
        base.GenerateBullet();
        tempBullet.transform.position -= extraOffset;
    }
}
