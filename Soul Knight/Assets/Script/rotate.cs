using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ø�����ʹͼƬ�ز���ת45��
public class rotate : MonoBehaviour
{
    public GameObject weapon;
    private void Update()
    {
        if(weapon) transform.rotation = Quaternion.Euler(0, 0, 45f *(weapon.GetComponent<weapon>().TellAngle()< 0 ? -1:1)) ;
    }
}
