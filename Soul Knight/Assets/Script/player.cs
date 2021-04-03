using UnityEngine;
using UnityEngine.UI;
using static scene;

public class player : MonoBehaviour
{
    [Header("属性")]
    const int maxHP=7,maxArmor =6,maxEnergy =200;
    float rate_critical=0.05f;//暴击率
    float speed = 6f,speed_up = 9f;
    const float cd_skill =10, t_skill = 4,cd_armor=1,cd_damage=1f;//技能cd,技能时间,护甲回复cd,无敌时间

    [Header("状态")]
    public int HP,armor,energy,coin;
    Vector2 direction;//移动的方向
    bool isMoving=false;
    bool skillPressed,pickPressed;
    public bool skillOn=false,skillReady=true;
    public float t_lastAttack,t_speedUp;//上次受到攻击到现在经过的时间,加速效果的时间
    public float count_armor=0;//护甲回复计时器，受伤会使计时器增大
    Text text_HP, text_armor, text_energy, text_coin;
    Image image_HP, image_armor, image_energy;

    [Header("系统")]
    Rigidbody2D rb;
    Animator anim;
    public GameObject Aweapon, Bweapon;
    GameObject pic_skillon, pic_skillready,dia_tp;
    AudioSource fx_skill,fx_skillready,fx_hurt,fx_energy,fx_switch;
    public int counter=0;
    public bool testMode=false;

    void Start()
    {
        Aweapon = FindFromAsset("weapon0");
        Bweapon = FindFromAsset("weapon1");
        Initialize();
        if (testMode) { speed = 20f; speed_up = 30f; }
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
        switch (collision.tag)
        {
            case "Room": t_lastAttack = -1f; break; //进入房间时获得无敌时间
            case "Tp":dia_tp.SetActive(true);break;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Weapon": dia_tp.SetActive(false); break;
            case "Tp": dia_tp.SetActive(false); break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Weapon":
                if(pickPressed)
                {
                    fx_switch.Play();
                    if (Aweapon.activeSelf)
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
                ;break;
            case "Tp":
                if (pickPressed) NextLevel(); break;
        }
    }

    void Initialize()
    {
        Text FindText(string name_back) => GameObject.Find("text_"+name_back).GetComponent<Text>();
        Image FindImage(string name_back) => GameObject.Find("image_"+name_back).GetComponent<Image>();

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        HP = maxHP;
        armor = maxArmor;
        energy = maxEnergy;
        dia_tp = FindFromUI("dia_tp"); 
        pic_skillon = FindFromUI("pic_skillon"); 
        pic_skillready = FindFromUI("pic_skillready");
        fx_skill = FindAudio("fx_skill");
        fx_skillready = FindAudio("fx_skillready");
        fx_hurt = FindAudio("fx_hurt"); 
        fx_energy = FindAudio("fx_energy");
        fx_switch = FindAudio("fx_switch");
        text_HP = FindText("HP"); 
        text_armor = FindText("armor"); 
        text_energy = FindText("energy"); 
        text_coin = FindText("coin");
        image_HP = FindImage("HP");
        image_armor = FindImage("armor");
        image_energy = FindImage("energy"); 
    }
    void InputCheck()
    {
        if(Input.GetKeyDown(KeyCode.E)) pickPressed=true;
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        isMoving = direction.magnitude > 0.1f;
        if (isMoving) direction =direction.normalized;
        skillPressed = Input.GetMouseButton(1);//右键使用技能
        if(Input.GetKeyDown(KeyCode.Q))//Q键切换武器
        {
            fx_switch.Play();
            if (Aweapon.activeSelf)
            {
                Aweapon.SetActive(false);
                Bweapon.SetActive(true);
                Bweapon.transform.position = transform.position;
            }
            else
            {
                Bweapon.SetActive(false);
                Aweapon.SetActive(true);
                Aweapon.transform.position = transform.position;
            }
        }
    }
    void PhysicsCheck()
    {
        if (isMoving) transform.localScale = new Vector3(direction.x < 0 ? -1 : 1, 1, 1);
        anim.SetBool("run", isMoving);
    }
    void Move()
    {
        rb.velocity = direction * (t_speedUp==0?speed:speed_up);
        if (rb.velocity.magnitude == 0) rb.velocity = new Vector2(0.01f, 0);//确保进行碰撞检测
    }
    void Effect()//处理技能、状态等
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
            pic_skillon.SetActive(true);
            pic_skillready.SetActive(false);
            fx_skill.Play();
        }
        if (testMode) { HP = maxHP;armor = maxArmor; }
        text_armor.text = maxArmor.ToString() + '/' + armor.ToString();
        image_armor.fillAmount = (float)armor / maxArmor;
    }
    void ResetSkill()
    {
        pic_skillready.SetActive(true);
        skillReady = true;
        fx_skillready.Play();
    }
    void EndSkillEffect()
    {
        pic_skillon.SetActive(false);
        skillOn = false;
    }

    public void GetDamage(int damage)
    {
        if (damage < 0) 
        {
            HP -= damage;
            if (HP > maxHP) HP = maxHP;
            return;
        }
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
                    GameObject.Find("UI").SendMessage("Mute");
                    speed = 0;
                    anim.SetBool("dead", true);
                    Destroy(Aweapon);Destroy(Bweapon);
                    Invoke(nameof(Die), 3f);
                }
            }
        }
        if (HP < 0) HP = 0;
        text_HP.text = maxHP.ToString() + '/' + HP.ToString();
        image_HP.fillAmount = (float)HP / maxHP;
    }
    public void CostEnergy(int cost)
    {
        if (cost < 0) fx_energy.Play();//吸收能量时播放音效
        energy -= cost;
        if (energy > maxEnergy) energy = maxEnergy;
        text_energy.text = maxEnergy.ToString() + '/' + energy.ToString();
        image_energy.fillAmount = (float)energy / maxEnergy;
    }
    public void SpendCoin(int num)
    {
        coin -= num;
        text_coin.text = coin.ToString();
    }
    protected void Die()
    {
        Destroy(this.gameObject);
        Exit();
    }
    public void SpeedUp(float time) => t_speedUp = time;
    public bool TellCritical() => Random.value <= rate_critical;
    public bool TellSkillOn() => skillOn;
    public int TellEnergy() => energy;
}