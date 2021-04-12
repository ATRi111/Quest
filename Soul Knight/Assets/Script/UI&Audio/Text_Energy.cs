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
        CEventSystem.EnergyChanged += OnEvent;
        text = GetComponent<Text>();

    }
    protected void OnEvent(int maxvalue,int value)
    {
        text.text = maxvalue.ToString() + "/" + value.ToString();
    }
    protected void OnDestroy()
    {
        CEventSystem.EnergyChanged -= OnEvent;
    }
}
