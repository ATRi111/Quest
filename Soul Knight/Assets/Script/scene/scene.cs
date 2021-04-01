using UnityEngine;
using UnityEngine.SceneManagement;
public class scene : MonoBehaviour
{
    protected GameObject door;
    protected static AudioSource bgm;
    protected virtual void Start()
    {
        Random.InitState(System.DateTime.Now.Second);
        door = GameObject.Find("door");
        door.SetActive(false);
        bgm = FindAudio("bgm");
    }

    public static AudioSource FindAudio(string name)=> GameObject.Find(name).GetComponent<AudioSource>();
    public static void Exit()
    {
        bgm.Stop();
        SceneManager.LoadScene("MENU");
    }
}
