using System.Collections.Generic;
using UnityEngine;
using static CSceneManager;
using static CAudioController;

public class CRoom : MonoBehaviour
{
    private static Quaternion s_ZeroQuaternion = Quaternion.Euler(0, 0, 0);
    protected int SPACE = 169;                    //一个房间的格数(除去边缘)
    protected int LENGTH = 13;                    //一个房间的边长（除去边缘）

    public int m_EnemyNum,m_BoxNum,m_Num;       //敌人、箱子数量和总数量
    public float X1, Y1, W, H;                  //房间范围
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
        GenerateEnemies();  //加载场景时即生成敌人，等Player进入后再激活
        Takarabox = GenerateFromAsset("takarabox", m_RandomPos[m_Num], s_ZeroQuaternion);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayAudio("fx_door");
            Door.SetActive(true);   //关门
            ActivateEnemies();
        }
    }

    private void GenerateEnemies()
    {
        //格子编号转位置
        Vector3 Order2Pos(int order)    
        {
            float x = order / LENGTH + 1.5f, y = order % LENGTH + 1.5f;
            return new Vector3(X1 + x * W / (LENGTH + 2), Y1 + y * H / (LENGTH + 2));
        }
        //控制不同怪物出现概率
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
        bool[] map = new bool[SPACE];   //避免生成重复随机数
        
        for (int i = 0; i < m_Num+1; i++)   //多生成一个随机位置，用于生成宝箱
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
            tempName = "enemy" + RandomID();    //怪物的对象名只有最后的数字不同
            m_Enemies.Add(GenerateFromAsset(tempName, m_RandomPos[i], s_ZeroQuaternion));
        }
    }
    private void ActivateEnemies()
    {
        for (int i = 0; i < m_BoxNum; i++)
        {
            m_Enemies[i].SetActive(true);   //箱子不需要连接到房间
        }
        for (int i = m_BoxNum; i < m_Num; i++)
        {
            m_Enemies[i].SetActive(true);
            m_Enemies[i].SendMessage("ConnectToRoom", this.gameObject);
        }
    }
    //离开房间
    private void GetOut()   
    {
        Takarabox.SetActive(true);  //生成宝箱
        Door.SetActive(false);  //开门
        PlayAudio("fx_door");
        Destroy(this.gameObject);
    }
    public void EnemyDie()
    {
        m_EnemyNum--;
        if (m_EnemyNum == 0) GetOut();
    }
}
