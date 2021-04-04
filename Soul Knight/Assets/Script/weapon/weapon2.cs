using UnityEngine;

public class weapon2 : weapon
{
    public bool testMode=false;
    protected override void Start()
    {
        base.Start();
        text = "¼«¹â";
        cd_shoot = 1000;
        cost = 4;
        deflectLevel = 0f;
        speed_shoot = 40;
        fx_weapon = scene.FindAudio("fx_weapon2");
        if (testMode) { cd_shoot = 100; cost = 0; }
    }
}
