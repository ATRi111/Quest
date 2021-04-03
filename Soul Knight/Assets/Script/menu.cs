using UnityEngine;
using UnityEngine.Audio;
using static UnityEngine.SceneManagement.SceneManager;
using static scene;
public class menu : MonoBehaviour
{
    GameObject pausemenu;
    GameObject help;
    public AudioMixer Mixer;
    AudioSource fx_menu;
    bool inGame;//处于游戏中（而不是开始界面,开始界面没有UI对象）

    private void Start()
    {
        inGame = GetActiveScene().buildIndex>0;
        if (inGame)
        {
            Mixer.SetFloat("main", -5);
            Mixer.SetFloat("bgm", -20);
            Mixer.SetFloat("fx", -20);
            pausemenu = FindFromUI("pausemenu");
            help = FindFromUI("help");
            fx_menu = FindAudio("fx_menu");
        }
    }
    private void Update()
    {
        if (inGame)
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
    public void SetmainVolume(float volume) => Mixer.SetFloat("main", -volume*volume/125f);
    public void SetfxVolume(float volume) => Mixer.SetFloat("fx", -volume * volume / 125f);
    public void SetbgmVolume(float volume) => Mixer.SetFloat("bgm", -volume * volume / 125f);
    public void Mute() => Mixer.SetFloat("main", -80);
}
