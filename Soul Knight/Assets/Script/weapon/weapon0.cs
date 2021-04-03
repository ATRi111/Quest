using UnityEngine;

public class weapon0 : weapon
{
    public bool testMode=false;
    protected override void Start()
    {
        base.Start();
        Pick();
        Name = "Ñ©ºü";
        cd_shoot = 200;
        bulletOffsetDistance = 0.5f;
        cost = 0;
        deflectLevel = 5f;
        fx_weapon = GameObject.Find("fx_weapon0").GetComponent<AudioSource>();
        if (testMode) cd_shoot = 5;
    }
}
