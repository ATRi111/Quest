using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.SceneManagement.SceneManager;
using UnityEngine.UI;
using static scene;
using static UnityEngine.PlayerPrefs;

public class menu : MonoBehaviour
{
    GameObject pausemenu;
    GameObject help;
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
            if(sceneIndex==1)
            {
                ResetData();
                mixer.SetFloat("main", -5);
                mixer.SetFloat("bgm", -20);
                mixer.SetFloat("fx", -20);
            }
            pausemenu = FindFromUI("pausemenu");
            help = FindFromUI("help");
            fx_menu = FindAudio("fx_menu");
            textbox = panel.transform.Find("Text").GetComponent<Text>();
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
    public void Play() => LoadScene(1);
    public void Exit() => LoadScene(0);
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

    public void ResetData()
    {
        SetString("Aweapon", "weapon0");
        SetString("Bweapon", "weapon1");
        SetInt("HP", 7);
        SetInt("armor", 6);
        SetInt("energy", 200);
        SetInt("coin", 0);
        SetInt("usingA", 1);
    }
}
