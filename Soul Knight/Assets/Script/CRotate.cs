using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用父物体使图片素材旋转45°
public class CRotate : MonoBehaviour
{
    public GameObject weapon;
    private void Update()
    {
        if(weapon) transform.rotation = Quaternion.Euler(0, 0, 45f *(weapon.GetComponent<CWeapon>().TellAngle()< 0 ? -1:1)) ;
    }
}
