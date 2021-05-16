using Public;
using UnityEngine;
using UnityEngine.UI;

public class Text_HP:MonoBehaviour
{
    private Text text;
    protected void Awake()
    {
        text = GetComponent<Text>();
    }
    private void OnEnable()
    {
        CEventSystem.Instance.AddLisenter<int, int>(EEventType.HPChanged, OnEvent);
    }
    private void OnDisable()
    {
        CEventSystem.Instance.RemoveListener<int, int>(EEventType.HPChanged, OnEvent);
    }
    protected void OnEvent(int maxvalue, int value)
    {
        text.text = maxvalue.ToString() + "/" + value.ToString();
    }
}
