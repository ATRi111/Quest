using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using static UnityEngine.SceneManagement.SceneManager;
public class menu : MonoBehaviour
{
    GameObject pausemenu;
    GameObject help;
    public AudioMixer Mixer;
    bool inGame;//������Ϸ�У������ǿ�ʼ����,��ʼ����û��UI����

    private void Start()
    {
        inGame = GetActiveScene().buildIndex>0;
        if (inGame)
        {
            Mixer.SetFloat("main", -5);
            Mixer.SetFloat("bgm", -20);
            Mixer.SetFloat("fx", -20);
            pausemenu = GameObject.Find("UI").transform.Find("pausemenu").gameObject;
            help = GameObject.Find("UI").transform.Find("help").gameObject;
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
        Time.timeScale = 0;
        pausemenu.SetActive(true);
    }
    public void Continue()
    {
        Time.timeScale = 1;
        pausemenu.SetActive(false);
    }
    public void OpenHelp()
    {
        Time.timeScale = 0;
        help.SetActive(true);
    }
    public void CloseHelp()
    {
        Time.timeScale = 1;
        help.SetActive(false);
    }
    public void SetmainVolume(float volume) => Mixer.SetFloat("main", -volume*volume/125f);
    public void SetfxVolume(float volume) => Mixer.SetFloat("fx", -volume * volume / 125f);
    public void SetbgmVolume(float volume) => Mixer.SetFloat("bgm", -volume * volume / 125f);
    public void Mute() => Mixer.SetFloat("main", -80);
}
