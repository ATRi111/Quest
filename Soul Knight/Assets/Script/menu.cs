using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.SceneManagement.SceneManager;
using UnityEngine.UI;
using static scene;
using static UnityEngine.PlayerPrefs;

public class menu : MonoBehaviour
{
    public GameObject pausemenu,help;
    public AudioMixer mixer;
    public GameObject panel;
    Text textbox;
    AudioSource fx_menu;
    int sceneIndex;

    private void Start()
    {
        sceneIndex = GetActiveScene().buildIndex;
        if (sceneIndex>0)
        {
            fx_menu = FindAudio("fx_menu");
            textbox = panel.transform.Find("Text").GetComponent<Text>();
            if (sceneIndex == 1) OpenHelp();
        }
    }
    private void Update()
    {
        if (sceneIndex>0)
        {   
            if(Input.GetKeyDown(KeyCode.Escape)&&!help.activeSelf)
                if (pausemenu.activeSelf) Continue(); 
                else Pause();
            if (Input.GetKeyDown(KeyCode.P)&&!pausemenu.activeSelf)
                if (help.activeSelf) CloseHelp();
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
        fx_menu.Play();
        Time.timeScale = 0;
        pausemenu.SetActive(true);
    }
    public void Continue()
    {
        fx_menu.Play();
        Time.timeScale = 1;
        pausemenu.SetActive(false);
    }
    public void OpenHelp()
    {
        fx_menu.Play();
        Time.timeScale = 0;
        help.SetActive(true);
    }
    public void CloseHelp()
    {
        fx_menu.Play();
        Time.timeScale = 1;
        help.SetActive(false);
    }
    public void SetmainVolume(float volume) => mixer.SetFloat("main", -volume*volume/125f);
    public void SetfxVolume(float volume) => mixer.SetFloat("fx", -volume * volume/125f);
    public void SetbgmVolume(float volume) => mixer.SetFloat("bgm", -volume * volume/125f);
    public void Mute() => mixer.SetFloat("main", -80);
    public void OpenText(string text)
    {
        panel.SetActive(true);
        textbox.text = text;
    }
    public void CloseText() => panel.SetActive(false);

    public static void ResetData()
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
