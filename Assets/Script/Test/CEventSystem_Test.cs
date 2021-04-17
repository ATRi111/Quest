using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Public;
using System;

public interface IObeserver
{
    void OnEvent(object arg1);
}

public class CEventSystem_Test : CSigleton<CEventSystem_Test>
{
    private Dictionary<EEventType, List<IObeserver>> m_Event = new Dictionary<EEventType, List<IObeserver>>();
    public void AddLisenter(EEventType eventType,IObeserver obeserver)
    {
        if (m_Event[eventType] == null) m_Event.Add(eventType, new List<IObeserver>());
        m_Event[eventType].Add(obeserver);
    }
    public void RemoveListener(EEventType eventType, IObeserver obeserver)
    {
        if (m_Event[eventType] == null) return;
        m_Event[eventType].Remove(obeserver);
    } 
    public void ActivateEvent(EEventType eventType,object arg1)
    {
        if (m_Event[eventType] == null) return;
        foreach (IObeserver item in m_Event[eventType])
            item.OnEvent(arg1);
    }
}



