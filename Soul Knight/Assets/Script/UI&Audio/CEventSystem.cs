using UnityEngine;
using Public;
using System;

public class CEventSystem:CSigleton<CEventSystem>
{
    //�¼�������Ҫ��Awake��OnDestroyʱ�޸�ί�У��¼�������Ҫ����ί��
    public static Action<int, int> HPChanged;        //���ֵ,��ǰֵ
    public static Action<int, int> ArmorChanged;   //���ֵ,��ǰֵ
    public static Action<int, int> EnergyChanged;  //���ֵ,��ǰֵ
    public static Action<int> CoinChanged;         //��ǰֵ
    public static Action ShootStart;
}


