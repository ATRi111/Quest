using UnityEngine.UI;
using UnityEngine;

public class Button_Play : MonoBehaviour
{
    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(CSceneManager.NextLevel);
    }
    
}
