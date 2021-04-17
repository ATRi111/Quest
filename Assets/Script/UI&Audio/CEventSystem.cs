using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Public;
using System;
using static UnityEngine.PlayerPrefs;
public enum EEventType
{
    HPChanged,
    ArmorChanged,      
    EnergyChanged,      
    CoinChanged,           
    ShootStart,
    SceneLoaded,
}

public class CEventSystem:CSigleton<CEventSystem>
{
    private readonly Dictionary<EEventType, Type> m_EventDict = new Dictionary<EEventType, Type>()
    {
        {EEventType.HPChanged,typeof(Action<int,int>)},
        {EEventType.ArmorChanged,typeof(Action<int,int>)},
        {EEventType.EnergyChanged,typeof(Action<int,int>) },
        {EEventType.CoinChanged,typeof(Action<int>)},
        {EEventType.ShootStart,typeof(Action)},
        {EEventType.SceneLoaded,typeof(Action<int>)}
    };
    private readonly Dictionary<EEventType,Delegate> m_Event=new Dictionary<EEventType, Delegate>();

    protected override void Awake()
    {
        foreach (EEventType key in m_EventDict.Keys)
        {
            m_Event.Add(key, null);
        }
        base.Awake();
        CEventSystem.Instance.AddLisenter<int>(EEventType.SceneLoaded, OnSceneLoaded);
    }

    private bool TypeCheck(EEventType eventType, Type methodType)
    {
        if (m_EventDict[eventType] != methodType)
        {
            Debug.Log("响应方法和事件类型不匹配");
            return false;
        }
        return true;
    }

    public void AddLisenter<T1>(EEventType eventType,Action<T1> listenerMethod)
    {
        if (TypeCheck(eventType,listenerMethod.GetType()))
            m_Event[eventType] = (Action<T1>)m_Event[eventType] + listenerMethod;
    }
    public void AddLisenter<T1,T2>(EEventType eventType, Action<T1,T2> listenerMethod)
    {
        if (TypeCheck(eventType, listenerMethod.GetType()))
            m_Event[eventType] = (Action<T1,T2>)m_Event[eventType] + listenerMethod;
    }

    public void RemoveListener<T1>(EEventType eventType,Action<T1> listenerMethod)
    {
        if (TypeCheck(eventType, listenerMethod.GetType()))
            m_Event[eventType] = (Action<T1>)m_Event[eventType]-listenerMethod;
    }
    public void RemoveListener<T1,T2>(EEventType eventType, Action<T1,T2> listenerMethod)
    {
        if (TypeCheck(eventType, listenerMethod.GetType()))
            m_Event[eventType] = (Action<T1,T2>)m_Event[eventType] - listenerMethod;
    }

    public void ActivateEvent<T1>(EEventType eventType,T1 arg1)
    {
        if (TypeCheck(eventType, typeof(Action<T1>)))
            (m_Event[eventType] as Action<T1>)?.Invoke(arg1);
    }
    public void ActivateEvent<T1,T2>(EEventType eventType, T1 arg1,T2 arg2)
    {
        if (TypeCheck(eventType, typeof(Action<T1,T2>)))
            (m_Event[eventType] as Action<T1,T2>)?.Invoke(arg1,arg2);
    }

    protected override void OnSceneLoaded(int index)
    {
        if (index == 0) ResetData();
    }
    private static void ResetData()
    {
        SetInt("Aweapon", 0);
        SetInt("Bweapon", 3);
        SetInt("HP", 7);
        SetInt("armor", 6);
        SetInt("energy", 200);
        SetInt("coin", 0);
        SetInt("usingA", 1);
        SetFloat("main", -5f);
        SetFloat("fx", -20f);
        SetFloat("bgm", -20f);
    }
}