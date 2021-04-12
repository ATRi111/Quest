using System.Collections.Generic;
using UnityEngine;
using static CSceneManager;
using static CAudioController;

public class CRoom : MonoBehaviour
{
    private static Quaternion s_ZeroQuaternion = Quaternion.Euler(0, 0, 0);
    protected int SPACE = 169;                    //һ������ĸ���(��ȥ��Ե)
    protected int LENGTH = 13;                    //һ������ı߳�����ȥ��Ե��

    public int m_EnemyNum,m_BoxNum,m_Num;       //���ˡ�����������������
    public float X1, Y1, W, H;                  //���䷶Χ
    private GameObject Door,Takarabox;
    private List<Vector3> m_RandomPos=new List<Vector3>();
    private List<GameObject> m_Enemies=new List<GameObject>();
    
    private void Start()
    {
        m_BoxNum = Random.Range(5, 31);
        m_EnemyNum = Random.Range(5, 8);
        m_Num = m_BoxNum + m_EnemyNum;
        W = transform.localScale.x;
        H = transform.localScale.y;
        X1 = transform.position.x - W*0.5f;
        Y1 = transform.position.y - H*0.5f;
        Door = GameObject.Find("map").transform.Find("door").gameObject;
        GenerateEnemies();  //���س���ʱ�����ɵ��ˣ���Player������ټ���
        Takarabox = GenerateFromAsset("takarabox", m_RandomPos[m_Num], s_ZeroQuaternion);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayAudio("fx_door");
            Door.SetActive(true);   //����
            ActivateEnemies();
        }
    }

    private void GenerateEnemies()
    {
        //���ӱ��תλ��
        Vector3 Order2Pos(int order)    
        {
            float x = order / LENGTH + 1.5f, y = order % LENGTH + 1.5f;
            return new Vector3(X1 + x * W / (LENGTH + 2), Y1 + y * H / (LENGTH + 2));
        }
        //���Ʋ�ͬ������ָ���
        string RandomID()   
        {
            int num = Random.Range(0,81);
            if (num < 21) return "0";
            else if (num < 41) return "1";
            else if (num < 61) return "2";
            else if (num < 76) return "3";
            else if (num < 86) return "4";
            else return "0";
        }
        int tempPos;
        string tempName;
        bool[] map = new bool[SPACE];   //���������ظ������
        
        for (int i = 0; i < m_Num+1; i++)   //������һ�����λ�ã��������ɱ���
        {
            for (; ; )
            {
                tempPos = Random.Range(0, SPACE);
                if (!map[tempPos]) { map[tempPos] = true;break; }
            }
            m_RandomPos.Add(Order2Pos(tempPos));
        }
        for (int i = 0; i < m_BoxNum; i++)
        {
            m_Enemies.Add(GenerateFromAsset("box", m_RandomPos[i], s_ZeroQuaternion));
        }
        for (int i = m_BoxNum; i < m_Num; i++)
        {
            tempName = "enemy" + RandomID();    //����Ķ�����ֻ���������ֲ�ͬ
            m_Enemies.Add(GenerateFromAsset(tempName, m_RandomPos[i], s_ZeroQuaternion));
        }
    }
    private void ActivateEnemies()
    {
        for (int i = 0; i < m_BoxNum; i++)
        {
            m_Enemies[i].SetActive(true);   //���Ӳ���Ҫ���ӵ�����
        }
        for (int i = m_BoxNum; i < m_Num; i++)
        {
            m_Enemies[i].SetActive(true);
            m_Enemies[i].SendMessage("ConnectToRoom", this.gameObject);
        }
    }
    //�뿪����
    private void GetOut()   
    {
        Takarabox.SetActive(true);  //���ɱ���
        Door.SetActive(false);  //����
        PlayAudio("fx_door");
        Destroy(this.gameObject);
    }
    public void EnemyDie()
    {
        m_EnemyNum--;
        if (m_EnemyNum == 0) GetOut();
    }
}
