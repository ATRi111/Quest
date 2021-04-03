using UnityEngine;

public class weapon2 : weapon
{
    public bool testMode=false;
    protected override void Start()
    {
        base.Start();
        Name = "¼«¹â";
        cd_shoot = 1000;
        bulletOffsetDistance = 0.5f;
        cost = 4;
        deflectLevel = 0f;
        speed_shoot = 40;
        fx_weapon = GameObject.Find("fx_weapon2").GetComponent<AudioSource>();
        if (testMode) { cd_shoot = 100; cost = 0; }
    }
}
