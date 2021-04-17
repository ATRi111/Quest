using Public;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text_Energy :MonoBehaviour
{
    private Text text;
    protected void Awake()
    {
        CEventSystem.Instance.AddLisenter<int, int>(EEventType.EnergyChanged, OnEvent);
        text = GetComponent<Text>();

    }
    protected void OnEvent(int maxvalue,int value)
    {
        text.text = maxvalue.ToString() + "/" + value.ToString();
    }
    protected void OnDestroy()
    {
        CEventSystem.Instance.RemoveListener<int, int>(EEventType.EnergyChanged, OnEvent);
    }
}
