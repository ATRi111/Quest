using UnityEngine;
using Public;
using static CAudioController;
using static UnityEngine.SceneManagement.SceneManager;
using UnityEngine.UI;
using static UnityEngine.PlayerPrefs;
using UnityEngine.Audio;

public class CMenu : CSigleton<CMenu>
{
    [SerializeField] private AudioMixer Mixer;
    public GameObject PauseMenu,Help;
    public GameObject Panel;
    private Text TextBox;
    private int SceneIndex;

    private void Start()
    {
        SceneIndex = GetActiveScene().buildIndex;
        if (SceneIndex>0)
        {
            TextBox = Panel.transform.Find("Text").GetComponent<Text>();
            if (SceneIndex == 1) OpenHelp();
        }
    }
    private void Update()
    {
        if (SceneIndex > 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !Help.activeSelf)
                if (PauseMenu.activeSelf) Continue();
                else Pause();
            if (Input.GetKeyDown(KeyCode.P) && !PauseMenu.activeSelf)
                if (Help.activeSelf) CloseHelp();
                else OpenHelp();
        }
    }
    public void Exit() => LoadScene(0);
    public void Quit() => Application.Quit();
    public void Pause()
    {
        CAudioController.PlayAudio("fx_menu");
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
    }
    public void Continue()
    {
        CAudioController.PlayAudio("fx_menu");
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }
    public void OpenHelp()
    {
        CAudioController.PlayAudio("fx_menu");
        Time.timeScale = 0;
        Help.SetActive(true);
    }
    public void CloseHelp()
    {
        CAudioController.PlayAudio("fx_menu");
        Time.timeScale = 1;
        Help.SetActive(false);
    }
    public void OpenText(string text)
    {
        Panel.SetActive(true);
        TextBox.text = text;
    }
    public void CloseText() => Panel.SetActive(false);

    public void SetmainVolume(float volume) => Mixer.SetFloat("main", -volume * volume / 125f);
    public void SetfxVolume(float volume) => Mixer.SetFloat("fx", -volume * volume / 125f);
    public void SetbgmVolume(float volume) => Mixer.SetFloat("bgm", -volume * volume / 125f);
    public void Mute() => Mixer.SetFloat("main", -80);
}
