using UnityEngine;

public class weapon0 : weapon
{
    public bool testMode=false;
    void Start()
    {
        Name = "ÆÆ¾ÉµÄÊÖÇ¹";
        color = Color.white;
        cd_shoot = 25;
        bulletOffsetDistance = 0.5f;
        cost = 0;
        fx_bullet = scene.FindAudio("fx_bullet0");
        deflectLevel = 5f;
        if (testMode) cd_shoot = 5;
    }
}
