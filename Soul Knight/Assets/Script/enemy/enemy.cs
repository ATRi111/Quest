using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{  
    [Header("����")]
    protected float speed=3f, speed_up;
    protected int damage=0;//�����˺�
    protected float cd_action = 3f, t_action = 2f;//�ж����,�ж�����ʱ��
    protected const float cd_move=2f,t_move=1;//����սʱ������ƶ��ļ��������ƶ���ʱ��

    [Header("״̬")]
    public int HP;
    public Vector2 pos;
    public float rad;
    public Vector2 drct=Vector2.zero;//�ƶ��ķ���
    public bool isMoving = false;
    public bool inBattle=false;//����ս��״̬
    public bool actionReady=true;//׼�����ж�
    public bool actionIng=false;//�����ж�
    public bool moveReady=true;//(��սʱ)׼��������ƶ�
    public float count_action=0;//�ж���ʱ��

    [Header("ϵͳ")]
    Rigidbody2D rb;
    protected AudioSource fx_attack;//��һ����
    protected Animator anim;
    GameObject player;
    Vector2 pos_player;

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    protected void FixedUpdate()
    {
        PhysicsCheck();
        Act();
    }

    void PhysicsCheck()
    {
        isMoving = rb.velocity.magnitude>0;
        pos = transform.position;
        if (isMoving) transform.localScale = new Vector3(drct.x < 0 ? -1 : 1, 1, 1);
        anim.SetBool("run", isMoving);
    }
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Wall": case "Door": case "Obstacle": drct*=0; break;
            case "Player": collision.gameObject.SendMessage("GetDamage", damage); drct *= 0; break;
        }
    }

    void Act()
    {
        if(!inBattle)
        {
            rb.velocity = drct * speed;
            if (!isMoving&&moveReady)//��սʱ����ƶ�
            {
                moveReady = false;
                rad = Random.Range(0, 2 * Mathf.PI);
                drct = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad));
                Invoke(nameof(ResetMove), cd_move);
                Invoke(nameof(EndMove), t_move);
            }
        }
        else 
        {
            if (actionReady)//ս��״̬�°��涨�ķ�ʽ�ж�
            {
                actionIng = true;
                actionReady = false;
                Invoke(nameof(ResetAction), cd_action);
                Invoke(nameof(EndAction), t_action);
            }  
            else if(actionIng)
            {
                DoAct();
            }
        }
    }
    public void GetDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            speed = 0;
            anim.SetBool("dead", true);
            Invoke(nameof(Die), 1);
        }
    }
    protected virtual void DoAct() { }
    protected void Die() => Destroy(this.gameObject);
    void ResetAction() => actionReady = true;
    void EndAction() => actionIng = false;
    void ResetMove() => moveReady = true;
    void EndMove()
    {
        if (!inBattle) drct = new Vector2(0, 0);
    }
}
