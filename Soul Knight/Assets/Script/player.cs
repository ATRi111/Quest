using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class player : MonoBehaviour
{
    [Header("����")]
    const int maxHP=7,maxArmor =6,maxEnergy =200;
    float criticalRate=0.5f;//������
    float speed = 5f,speed_up = 7.5f;
    float cd_skill =10, t_skill = 4,cd_armor=1,cd_damage=0.25f;//����cd,����ʱ��,���׻ظ�cd,�޵�ʱ��

    [Header("״̬")]
    public int HP,armor,energy;
    public Vector2 pos;
    public Vector2 drct;//�ƶ��ķ���
    public bool isMoving=false;
    bool usingA=true;//��ʹ������A
    bool skillPressed;//Ҫ��ʹ�ü���
    public bool skillOn=false,skillReady=true;
    public float t_lastAttack,t_speedUp;//�ϴ��ܵ����������ھ�����ʱ��,����Ч����ʱ��
    public float count_armor=0;//���׻ظ���ʱ�������˻�ʹ��ʱ������

    [Header("ϵͳ")]
    Rigidbody2D rb;
    Animator anim;
    public GameObject Aweapon;
    public GameObject Bweapon;
    public GameObject skill_picture1, skill_picture2;
    AudioSource bgm,fx_skill,fx_skillready,fx_hurt;
    public int counter=0;

    void Start()
    {
        Initialize();
        Aweapon.SendMessage("Pick");//��ȡ��ʼ����
        bgm.Play();
    }

    void Update()
    {
        InputCheck();
    }

    private void FixedUpdate()
    {
        PhysicsCheck();
        Move();
        Effect();
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Weapon":
                if(Input.GetButtonDown("Pick"))
                    {
                        if (usingA)
                        {
                            Aweapon.SendMessage("Discard");
                            Aweapon = collision.gameObject;
                            Aweapon.SendMessage("Pick");
                        }
                        else
                        {
                            Bweapon.SendMessage("Discard");
                            Bweapon = collision.gameObject;
                            Bweapon.SendMessage("Pick");
                        }
                    }
                    break;
            case "TP":
                if (nameof(collision) == "tp" && Input.GetButtonDown("Pick"))
                {

                }
                break;

        }
    }
    void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        HP = maxHP;
        armor = maxArmor;
        energy = maxEnergy;
        bgm = scene.FindAudio("bgm");
        fx_skill = scene.FindAudio("fx_skill");
        fx_skillready = scene.FindAudio("fx_skillready");
        fx_hurt = scene.FindAudio("fx_hurt");
    }
    void InputCheck()
    {
        drct = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        isMoving = drct.magnitude > 0;
        if (isMoving) drct =drct.normalized;
        skillPressed = Input.GetMouseButton(1);//�Ҽ�ʹ�ü���
    }
    void PhysicsCheck()
    {
        pos = transform.position;
        if (isMoving) transform.localScale = new Vector3(drct.x < 0 ? -1 : 1, 1, 1);
        anim.SetBool("run", isMoving);
    }
    void Move()
    {
        rb.velocity = drct * (t_speedUp==0?speed:speed_up);
    }
    void Effect()//�����ܡ�״̬��
    {
        t_lastAttack += Time.deltaTime;
        if (count_armor > 0) count_armor -= Time.deltaTime;
        else if(armor<maxArmor)
        {
            armor += 1;
        }
        if (skillPressed&&skillReady)
        {
            skillOn = true;
            skillReady = false;
            Invoke(nameof(EndSkillEffect), t_skill);
            Invoke(nameof(ResetSkill), cd_skill);
            skill_picture1.SetActive(false);
            skill_picture2.SetActive(true);
            fx_skill.Play();
        }
    }
    void ResetSkill()
    {
        counter++;
        skill_picture1.SetActive(true);
        skillReady = true;
    }
    void EndSkillEffect()
    {
        skill_picture2.SetActive(false);
        skillOn = false;
    }
    public void GetDamage(int damage)
    {
        t_lastAttack = 0;
        count_armor = 3;
        if (armor >= damage) armor -= damage;
        else
        {
            damage -= armor; 
            armor = 0;
        }
    }
    public bool TellCritical() => Random.value <= criticalRate;
    public bool TellSkillOn() => skillOn;
    public int TellEnergy() => energy;
    public void CostEnergy(int cost)=> energy -= cost;
    public void SpeedUp(float time) => t_speedUp = time;
}
