using UnityEngine;
using Public;
using static CAudioController;
using System.Collections.Generic;
using static UnityEngine.SceneManagement.SceneManager;

public class CSceneManager :CSigleton<CSceneManager>
{
    public static int m_Index=0;  //��ǰ�ص�index
    
    private void Start()
    {
        m_Index = GetActiveScene().buildIndex;
        Random.InitState(System.DateTime.Now.Second);
    }

    protected override void OnSceneLoaded(int index)
    {
        m_Index = index;
        StopAudio("bgm2"); StopAudio("bgm1");
        switch (m_Index)
        {  
            case 1:  CAudioController.PlayAudio("bgm1"); break;
            case 2:  CAudioController.PlayAudio("bgm2"); break;
        }
    }
    public static void NextLevel()
    {
        if (m_Index == 2)
        {
            LoadScene(0);
            CEventSystem.Instance.ActivateEvent<int>(EEventType.SceneLoaded, 0);
        }
        else
        {
            LoadScene(m_Index + 1);
            CEventSystem.Instance.ActivateEvent<int>(EEventType.SceneLoaded, m_Index + 1);
        }
    }
    public static void Exit()
    {
        LoadScene(0);
        CEventSystem.Instance.ActivateEvent<int>(EEventType.SceneLoaded, 0);
    }

    public static void GenerateEnergyPoint(Vector3 pos, int num = 1)
    {
        for (int i = 0; i < num; i++)
            GenerateFromAsset("energypoint", pos+ CTool.RandomVector3(), CTool.s_ZeroQuaternion).SetActive(true);
    }

    public static GameObject GenerateFromAsset(string name, Vector3 position, Quaternion quaternion)
    {
        GameObject temp;
        switch (name)
        {
            case "weapon3":
            case "weapon5"://����������ֻ����������壬����Ҫ�����������ɲ�����
                {
                    temp = GameObject.Find("asset").transform.Find(name).gameObject;
                    temp = Instantiate(temp, position, quaternion);
                    temp.SetActive(true);
                    temp = temp.transform.Find(name).gameObject;//�����屻�����ˣ���������û��
                }
                break;
            default:
                {
                    temp = GameObject.Find("asset").transform.Find(name).gameObject;
                    temp = Instantiate(temp, position, CTool.s_ZeroQuaternion);
                }
                break;
        }
        return temp;// ���ظ���Ʒ������
    }
    public static void GenerateCoinPoint(Vector3 pos, int num = 1)
    {
        for (int i = 0; i < num; i++)
            GenerateFromAsset("coinpoint", pos + CTool.RandomVector3(), CTool.s_ZeroQuaternion).SetActive(true); 
    }
}
