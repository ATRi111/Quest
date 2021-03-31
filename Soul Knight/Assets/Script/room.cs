using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class room : MonoBehaviour
{
    public int count_enemy;//��������
    public GameObject door;//������
    AudioSource fx_door;

    void Start()
    {
        fx_door = scene.FindAudio("fx_door");
        door.SetActive(false);//����
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
            door.SetActive(true);//����
            GenerateEnemies();
        }
    }

    void GenerateEnemies()
    {
        count_enemy = 0;
        Random.InitState((int)Time.time);
    }
    void GetOut()//�뿪����
    {
        door.SetActive(false);//����
        fx_door.Play();
        Destroy(this);
    }
}
