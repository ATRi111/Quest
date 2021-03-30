using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class player : MonoBehaviour
{
    [Header("属性")]
    int maxHP=8;
    int maxArmor=5;
    int maxEnergy=200;
    float criticalRate;//暴击率
    public float speed = 8f;

    [Header("状态")]
    public int HP;
    public int armor;
    public int energy;
    public Vector2 pos;
    public Vector2 direction;//移动的方向
    public Vector2 aim;//瞄准的方向
    public bool isMoving;
    public bool inRoom;

    [Header("系统")]
    Rigidbody2D rb;
    BoxCollider2D coll;
    Animator anim;

    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        InputCheck();
    }

    private void FixedUpdate()
    {
        PhysicsCheck();
        Move();

    }

    void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        HP = maxHP;
        armor = maxArmor;
        energy = maxEnergy;
    }
    void InputCheck()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        isMoving = direction.magnitude > 0;
        if (isMoving) direction /= direction.magnitude;
    }
    void PhysicsCheck()
    {
        pos = transform.position;
    }
    void Move()
    {
        if (isMoving) transform.localScale = new Vector3(direction.x < 0 ? -1 : 1, 1, 1);
        rb.velocity = direction * speed;
        anim.SetBool("run", isMoving);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Room"))//进入了房间
        {
            inRoom = true;
        }
    }
}
