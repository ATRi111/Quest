using Public;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text_Coin:MonoBehaviour
{
    private Text text;
    protected void Awake()
    {
        CEventSystem.CoinChanged += OnEvent;
        text = GetComponent<Text>();

    }
    protected void OnEvent(int value)
    {
        text.text = value.ToString();
    }
    protected void OnDestroy()
    {
        CEventSystem.CoinChanged -= OnEvent;
    }
}
