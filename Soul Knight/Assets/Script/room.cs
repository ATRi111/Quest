using System.Collections.Generic;
using UnityEngine;

public class room : MonoBehaviour
{
    public short count_enemy;//敌人数量
    public float x1, y1, w, h;//房间范围
    short n;//敌人和箱子的总数
    short min, max = 1;//随机生成id的范围(取不到max)
    const short space= 169,length=13;//一个房间的格数，边长(除去边缘)
    GameObject door;//房间门
    AudioSource fx_door;
    List<short> randomId=new List<short>();//负数表示箱子
    List<Vector3> randomPos=new List<Vector3>();
    List<GameObject> enemies=new List<GameObject>();
    Quaternion zeroQuaternion = Quaternion.Euler(0, 0, 0);
    
    void Start()
    {
        n = (short)Random.Range(20, 41);
        min = (short)((-n + 6) / 6 * max);
        w = transform.localScale.x;h = transform.localScale.y;
        x1 = transform.position.x - w*0.5f;y1 = transform.position.y - h*0.5f;
        door = GameObject.Find("map").transform.Find("door").gameObject;
        fx_door = scene.FindAudio("fx_door");
        GenerateEnemies();//加载场景时即生成敌人，等Player进入后再激活
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            fx_door.Play();
            door.SetActive(true);//关门
            ActivateEnemies();
        }
    }

    void GenerateEnemies()
    {
        Vector3 Order2Pos(int order)//格子编号转位置
        {
            float x = order / length+1.5f, y = order % length+1.5f;
            return new Vector3(x1+x * w / (length + 2), y1+y * h / (length + 2));
        }
        List<bool> map = new List<bool>(space);//避免随机数重复
        for (short i = 0; i < n; i++)
        {
            randomId.Add((short)Random.Range(min, max));
            randomPos.Add(Order2Pos(Random.Range(0, space)));
        }

        string tempName;
        for (short i = 0; i < n; i++)
        {
            if (randomId[i] < 0) enemies.Add(GameObject.Instantiate(GameObject.Find("asset").transform.Find("box").gameObject, randomPos[i], zeroQuaternion));
            else
            {
                tempName = "enemy" + randomId[i].ToString();
                enemies.Add(GameObject.Instantiate(GameObject.Find("asset").transform.Find(tempName).gameObject, randomPos[i], zeroQuaternion));
                count_enemy++;
            } 
        }
    }
    void ActivateEnemies()
    {
        for (short i = 0; i < n; i++)
        {
            enemies[i].SetActive(true);
            if(randomId[i]>=0) enemies[i].SendMessage("ConnectToRoom", this.gameObject);
        }
    }
    void GetOut()//离开房间
    {
        door.SetActive(false);//开门
        fx_door.Play();
        Destroy(this.gameObject);
    }
    public void EnemyDie()
    {
        count_enemy--;
        if (count_enemy == 0) GetOut();
    }
}
