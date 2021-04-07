using UnityEngine;

public class weapon1 : weapon
{
    protected override void Start()
    {
        base.Start();
        text = "ö±µ¯³å·æÇ¹";
        cd_shoot = 500;
        cost = 2;
        deflectLevel = 10f;
        fx_weapon = scene.FindAudio("fx_weapon1");
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
