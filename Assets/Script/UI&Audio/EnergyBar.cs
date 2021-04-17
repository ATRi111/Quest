using Public;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        CEventSystem.Instance.AddLisenter<int,int>(EEventType.EnergyChanged,OnEvent);
    }
    private void OnEvent(int maxvalue, int value)
    {
        image.fillAmount = (float)value / maxvalue;
    }
    private void OnDestroy()
    {
        CEventSystem.Instance.RemoveListener<int, int>(EEventType.EnergyChanged, OnEvent);
    }
}
