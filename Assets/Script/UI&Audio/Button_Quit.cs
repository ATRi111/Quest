using UnityEngine.UI;
using UnityEngine;

public class Button_Quit : MonoBehaviour
{
    private Button button;
    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(CSceneManager.Exit);
    }
    
}
