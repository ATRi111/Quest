using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.SceneManagement.SceneManager;
using UnityEngine.UI;
using static CSceneManager;
using static UnityEngine.PlayerPrefs;
using static CAudioController;

public class CMenu : MonoBehaviour
{
    public GameObject PauseMenu,Help;
    public AudioMixer Mixer;
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
        if (SceneIndex>0)
        {   
            if(Input.GetKeyDown(KeyCode.Escape)&&!Help.activeSelf)
                if (PauseMenu.activeSelf) Continue(); 
                else Pause();
            if (Input.GetKeyDown(KeyCode.P)&&!PauseMenu.activeSelf)
                if (Help.activeSelf) CloseHelp();
                else OpenHelp();
        }
    }
    public void Play()
    {
        ResetData();
        LoadScene(1);
    }
    public void Exit() => LoadScene(0);
    public void Quit() => Application.Quit();
    public void Pause()
    {
        PlayAudio("fx_menu");
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
    }
    public void Continue()
    {
        PlayAudio("fx_menu");
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }
    public void OpenHelp()
    {
        PlayAudio("fx_menu");
        Time.timeScale = 0;
        Help.SetActive(true);
    }
    public void CloseHelp()
    {
        PlayAudio("fx_menu");
        Time.timeScale = 1;
        Help.SetActive(false);
    }
    public void SetmainVolume(float volume) => Mixer.SetFloat("main", -volume*volume/125f);
    public void SetfxVolume(float volume) => Mixer.SetFloat("fx", -volume * volume/125f);
    public void SetbgmVolume(float volume) => Mixer.SetFloat("bgm", -volume * volume/125f);
    public void Mute() => Mixer.SetFloat("main", -80);
    public void OpenText(string text)
    {
        Panel.SetActive(true);
        TextBox.text = text;
    }
    public void CloseText() => Panel.SetActive(false);
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
