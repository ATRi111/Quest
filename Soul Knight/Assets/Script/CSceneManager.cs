using UnityEngine;
using UnityEngine.SceneManagement;
using Public;
using static CAudioController;
using System.Collections.Generic;
using static UnityEngine.SceneManagement.SceneManager;

public class CSceneManager : MonoBehaviour
{
    public static int m_NextIndex;//下一关的index
    public static Quaternion s_ZeroQuaternion = new Quaternion();

    private void Start()
    {
        m_NextIndex = GetActiveScene().buildIndex + 1;
        Random.InitState(System.DateTime.Now.Second);
        switch (m_NextIndex - 1)
        {
            case 1:StopAudio("bgm2"); PlayAudio("bgm1"); break;
            case 2:StopAudio("bgm1"); PlayAudio("bgm2"); break;
        }
    }
 
    public static GameObject FindFromUI(string name)
        => GameObject.Find("UI").transform.Find(name).gameObject;
    public static GameObject GenerateFromAsset(string name, Vector3 position, Quaternion quaternion)
    {
        GameObject temp;
        switch (name)
        {
            case "weapon3": case "weapon5"://其他程序里只会控制子物体，这里要将父物体生成并激活
                {
                    temp = GameObject.Find("asset").transform.Find(name).gameObject;
                    temp = Instantiate(temp, position, quaternion);
                    temp.SetActive(true);
                    temp = temp.transform.Find(name).gameObject;//父物体被启用了，而子物体没有
                }
                break;
            default:
                {
                    temp = GameObject.Find("asset").transform.Find(name).gameObject;
                    temp = Instantiate(temp, position, s_ZeroQuaternion);
                }
                break;
        }
        return temp;// 返回复制品的引用
    }
    public static void NextLevel()
    {
        if (m_NextIndex == 3) LoadScene(0);
        else LoadScene(m_NextIndex);
    }
    public static void Exit() => LoadScene(0);
    public static void GenerateEnergyPoint(Vector3 pos, int num = 1)
    {
        for (int i = 0; i < num; i++)
            GenerateFromAsset("energypoint", pos+new Vector3(Random.Range(1f,-1f),Random.Range(1f,-1f)), s_ZeroQuaternion).SetActive(true);
    }
    public static void GenerateCoinPoint(Vector3 pos, int num = 1)
    {
        for (int i = 0; i < num; i++)
            GenerateFromAsset("coinpoint", pos + new Vector3(Random.Range(1f, -1f), Random.Range(1f, -1f)), s_ZeroQuaternion).SetActive(true);
    }
    public static Vector2 Angle2Direction(float angle) 
        => new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
    public static float Direction2Angle(Vector2 direction)
        => Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
}
