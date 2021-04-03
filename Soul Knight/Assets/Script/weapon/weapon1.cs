using UnityEngine;

public class weapon1 : weapon
{
    public bool testMode=false;
    protected override void Start()
    {
        base.Start();
        Pick();
        Name = "ö±µ¯³å·æÇ¹";
        cd_shoot = 500;
        cost = 2;
        deflectLevel = 10f;
        fx_weapon = scene.FindAudio("fx_weapon1");
        if (testMode) { cd_shoot = 5; cost = 0; }
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
