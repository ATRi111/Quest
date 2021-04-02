using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.SceneManagement.SceneManager;
public class scene : MonoBehaviour
{
    public static int nextIndex;//下一关的index
    private void Start()
    {
        nextIndex = (GetActiveScene().buildIndex+1)%3;
        if (nextIndex == 2) GameObject.Find("UI").SendMessage("OpenHelp");
        Random.InitState(System.DateTime.Now.Second);
        GameObject.Find("audio").SetActive(true);
        FindAudio("bgm").Play();
    }
    public static AudioSource FindAudio(string name)=> GameObject.Find(name).GetComponent<AudioSource>();
    public static GameObject FindFromUI(string name) => GameObject.Find("UI").transform.Find(name).gameObject;
    public static void Restart()=> LoadScene(nextIndex);
    public static void NextLevel()
    {
        LoadScene(nextIndex);
        MoveGameObjectToScene(GameObject.FindWithTag("Player"), SceneManager.GetSceneByBuildIndex(nextIndex));
    }
    public static void Exit()=> LoadScene("MENU");
}
