using Public;
using UnityEngine;
using UnityEngine.UI;

public class Text_HP:MonoBehaviour
{
    private Text text;
    protected void Awake()
    {
        CEventSystem.HPChanged += OnEvent;
        text = GetComponent<Text>();
    }
    protected void OnEvent(int maxvalue, int value)
    {
        text.text = maxvalue.ToString() + "/" + value.ToString();
    }
    protected void OnDestroy()
    {
        CEventSystem.HPChanged -= OnEvent;
    }
}
