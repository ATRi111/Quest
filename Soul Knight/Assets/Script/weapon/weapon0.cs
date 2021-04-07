using UnityEngine;

public class weapon0 : weapon
{
    protected override void Start()
    {
        base.Start();
        text = "Ñ©ºü";
        cd_shoot = 250;
        cost = 0;
        deflectLevel = 5f;
        fx_weapon = scene.FindAudio("fx_weapon0");
    }
}
