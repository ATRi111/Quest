using Public;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
        CEventSystem.HPChanged += OnEvent;
    }
    private void OnEvent(int maxvalue,int value)
    {
        image.fillAmount = (float)value / maxvalue;
    }
    private void OnDestroy()
    {
        CEventSystem.HPChanged -= OnEvent;
    }
}
