using Public;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBar : MonoBehaviour
{
    Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        CEventSystem.EnergyChanged += OnEvent;
    }
    private void OnEvent(int maxvalue, int value)
    {
        image.fillAmount = (float)value / maxvalue;
    }
    private void OnDestroy()
    {
        CEventSystem.EnergyChanged -= OnEvent;
    }
}
