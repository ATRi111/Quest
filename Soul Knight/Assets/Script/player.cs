using UnityEngine;

public class player : MonoBehaviour
{
    [Header("����")]
    const short maxHP=7,maxArmor =6,maxEnergy =200;
    float criticalRate=0.5f;//������
    float speed = 5f,speed_up = 7.5f;
    const float cd_skill =10, t_skill = 4,cd_armor=1,cd_damage=0.5f;//����cd,����ʱ��,���׻ظ�cd,�޵�ʱ��

    [Header("״̬")]
    public short HP,armor,energy;
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
    public GameObject Aweapon,Bweapon;
    public GameObject skill_picture1, skill_picture2;
    public GameObject dia_pick, dia_tp;
    AudioSource fx_skill,fx_skillready,fx_hurt;
    public short counter=0;
    public bool testMode;

    void Start()
    {
        Initialize();
        Aweapon.SendMessage("Pick");//��ȡ��ʼ����
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Room")) t_lastAttack = -1f; //���뷿��ʱ����޵�ʱ��
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Weapon":
                counter++;
                dia_pick.SetActive(true);
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
            case "Tp":
                dia_tp.SetActive(true);
                if (nameof(collision) == "tp" && Input.GetButtonDown("Pick"))
                {

                }
                break;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Weapon":dia_tp.SetActive(false);break;
            case "Tp":dia_tp.SetActive(false);break;
        }
    }
    void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        HP = maxHP;
        armor = maxArmor;
        energy = maxEnergy;
        fx_skill = scene.FindAudio("fx_skill");
        fx_skillready = scene.FindAudio("fx_skillready");
        fx_hurt = scene.FindAudio("fx_hurt");
        if (testMode){armor = 1000;speed = 20f;speed_up = 30f;energy = 10000;}
    }
    void InputCheck()
    {
        drct = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        isMoving = drct.magnitude > 0.1f;
        if (isMoving) drct =drct.normalized;
        skillPressed = Input.GetMouseButton(1);//�Ҽ�ʹ�ü���
    }
    void PhysicsCheck()
    {
        if (isMoving) transform.localScale = new Vector3(drct.x < 0 ? -1 : 1, 1, 1);
        anim.SetBool("run", isMoving);
    }
    void Move()
    {
        rb.velocity = drct * (t_speedUp==0?speed:speed_up);
    }
    void Effect()//�����ܡ�״̬��
    {
        t_lastAttack += Time.fixedDeltaTime;
        if(t_speedUp>0) t_speedUp -= Time.fixedDeltaTime;
        if (count_armor > 0) count_armor -= Time.fixedDeltaTime;
        else if (armor < maxArmor)
        {
            armor++;
            count_armor = cd_armor;
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
        fx_skillready.Play();
    }
    void EndSkillEffect()
    {
        skill_picture2.SetActive(false);
        skillOn = false;
    }
    public void GetDamage(short damage)
    {
        if (t_lastAttack > cd_damage)
        {
            fx_hurt.Play();
            t_lastAttack = 0f;
            count_armor = 3f;
            if (armor >= damage) armor -= damage;
            else
            {
                damage -= armor;
                armor = 0;
                HP -= damage;
                if(HP<= 0)
                {
                    speed = 0;
                    anim.SetBool("dead", true);
                    Destroy(Aweapon);Destroy(Bweapon);
                    Invoke(nameof(Die), 2f);
                }
            }
        }
    }
    protected void Die() => Destroy(this.gameObject);
    public void CostEnergy(short cost) => energy -= cost;
    public void SpeedUp(float time) => t_speedUp = time;
    public bool TellCritical() => Random.value <= criticalRate;
    public bool TellSkillOn() => skillOn;
    public short TellEnergy() => energy;
}
