using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class room : MonoBehaviour
{
    public int count_enemy;//敌人数量
    public GameObject door;//房间门

    // Start is called before the first frame update
    void Start()
    {
        door.SetActive(false);//开门
        count_enemy = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if(count_enemy==0) Invoke(nameof(GetOut), 5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "knight")
        { 
            door.SetActive(true);//关门
            GenerateEnemies();
        }
    }

    void GenerateEnemies()
    {
        count_enemy = 0;
        Random.InitState((int)Time.time);
    }
    void GetOut()//离开房间
    {
        door.SetActive(false);//开门
        Destroy(this);
    }
}
