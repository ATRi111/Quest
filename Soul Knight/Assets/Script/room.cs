using System.Collections.Generic;
using UnityEngine;
using static scene;

public class room : MonoBehaviour
{
    public int count_enemy,count_box,n;//���ˡ�����������������
    public float x1, y1, w, h;//���䷶Χ
    const int space= 169,length=13;//һ������ĸ������߳�(��ȥ��Ե)
    GameObject door,takarabox;
    AudioSource fx_door;
    List<Vector3> randomPos=new List<Vector3>();
    List<GameObject> enemies=new List<GameObject>();
    Quaternion zeroQuaternion = Quaternion.Euler(0, 0, 0);
    
    void Start()
    {
        count_box = Random.Range(5, 31);
        count_enemy = Random.Range(5, 8);
        n = count_box + count_enemy;
        w = transform.localScale.x;h = transform.localScale.y;
        x1 = transform.position.x - w*0.5f;y1 = transform.position.y - h*0.5f;
        door = GameObject.Find("map").transform.Find("door").gameObject;
        fx_door = scene.FindAudio("fx_door");
        GenerateEnemies();//���س���ʱ�����ɵ��ˣ���Player������ټ���
        takarabox = GenerateFromAsset("takarabox", randomPos[n], zeroQuaternion);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            fx_door.Play();
            door.SetActive(true);//����
            ActivateEnemies();
        }
    }

    void GenerateEnemies()
    {
        Vector3 Order2Pos(int order)//���ӱ��תλ��
        {
            float x = order / length + 1.5f, y = order % length + 1.5f;
            return new Vector3(x1 + x * w / (length + 2), y1 + y * h / (length + 2));
        }
        string RandomID()//���Ʋ�ͬ������ָ���
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
        bool[] map = new bool[space];//���������ظ������
        
        for (int i = 0; i < n+1; i++) //������һ�����λ�ã��������ɱ���
        {
            for (; ; )
            {
                tempPos = Random.Range(0, space);
                if (!map[tempPos]) { map[tempPos] = true;break; }
            }
            randomPos.Add(Order2Pos(tempPos));
        }
        for (int i = 0; i < count_box; i++)
        {
            enemies.Add(GenerateFromAsset("box", randomPos[i], zeroQuaternion));
        }
        for (int i = count_box; i < n; i++)
        {
            tempName = "enemy" + RandomID();//����Ķ�����ֻ���������ֲ�ͬ
            enemies.Add(GenerateFromAsset(tempName, randomPos[i], zeroQuaternion));
        }
    }
    void ActivateEnemies()
    {
        for (int i = 0; i < count_box; i++)
        {
            enemies[i].SetActive(true);//���Ӳ���Ҫ���ӵ�����
        }
        for (int i = count_box; i < n; i++)
        {
            enemies[i].SetActive(true);
            enemies[i].SendMessage("ConnectToRoom", this.gameObject);
        }
    }
    void GetOut()//�뿪����
    {
        takarabox.SetActive(true);//���ɱ���
        door.SetActive(false);//����
        fx_door.Play();
        Destroy(this.gameObject);
    }
    public void EnemyDie()
    {
        count_enemy--;
        if (count_enemy == 0) GetOut();
    }
}
