using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class room : MonoBehaviour
{
    public int count_enemy;//敌人数量
    public GameObject door;//房间门
    AudioSource fx_door;

    void Start()
    {
        fx_door = scene.FindAudio("fx_door");
        door.SetActive(false);//开门
        count_enemy = -1;
    }

    void Update()
    {
        if(count_enemy==0) Invoke(nameof(GetOut), 5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "knight")
        {
            fx_door.Play();
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
        fx_door.Play();
        Destroy(this);
    }
}
