using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class room : MonoBehaviour
{
    public int count_enemy;//��������
    public GameObject door;//������

    // Start is called before the first frame update
    void Start()
    {
        door.SetActive(false);//����
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
        Destroy(this);
    }
}
