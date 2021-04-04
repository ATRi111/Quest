using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using static UnityEngine.SceneManagement.SceneManager;

public class scene : MonoBehaviour
{
    public static int nextIndex;//下一关的index
    public static Quaternion zeroQuaternion = new Quaternion();
    player player;

    private void Start()
    {
        nextIndex = (GetActiveScene().buildIndex+1)%3;
        if (nextIndex == 2) GameObject.Find("UI").SendMessage("OpenHelp");
        Random.InitState(System.DateTime.Now.Second);
        GameObject.Find("audio").SetActive(true);
        FindAudio("bgm").Play();
        player = GameObject.FindWithTag("Player").GetComponent<player>();
    }
    public static AudioSource FindAudio(string name)=> GameObject.Find(name).GetComponent<AudioSource>();
    public static GameObject FindFromUI(string name) => GameObject.Find("UI").transform.Find(name).gameObject;
    public static GameObject FindFromAsset(string name)
    {
        return GameObject.Find("asset").transform.Find(name).gameObject;
    }
    public static void NextLevel()
    {
        LoadScene(nextIndex);
        MoveGameObjectToScene(GameObject.FindWithTag("Player"), SceneManager.GetSceneByBuildIndex(nextIndex));
    }
    public static void Exit()=> LoadScene(0);
    public static void GenerateEnergyPoint(Vector3 pos,int num=1)
    {
        for (int i = 0; i < num; i++)
            Instantiate(FindFromAsset("energypoint"), 
                pos+new Vector3(Random.Range(1f,-1f),Random.Range(1f,-1f)), zeroQuaternion).SetActive(true);
    }
    public static void GenerateCoinPoint(Vector3 pos, int num = 1)
    {
        for (int i = 0; i < num; i++)
            Instantiate(FindFromAsset("coinpoint"), pos + new Vector3(Random.Range(1f, -1f), Random.Range(1f, -1f)), zeroQuaternion).SetActive(true);
    }
    public static Vector2 Angle2Direction(float angle) => new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
    public static float Direction2Angle(Vector2 direction)=> Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
}
