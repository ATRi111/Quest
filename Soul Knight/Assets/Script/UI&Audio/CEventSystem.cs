using UnityEngine;
using Public;
using System;

public class CEventSystem:CSigleton<CEventSystem>
{
    //事件接收者要在Awake和OnDestroy时修改委托，事件触发者要调用委托
    public static Action<int, int> HPChanged;        //最大值,当前值
    public static Action<int, int> ArmorChanged;   //最大值,当前值
    public static Action<int, int> EnergyChanged;  //最大值,当前值
    public static Action<int> CoinChanged;         //当前值
    public static Action ShootStart;
}


