using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon0 : weapon
{
    void Start()
    {
        Name = "�ƾɵ���ǹ";
        color = Color.white;
        shoot_cd = 25;
        bulletOffsetDistance = 0.5f;
        cost = 0;
        fx_bullet = scene.FindAudio("fx_bullet0");
    }
}
