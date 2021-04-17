using UnityEngine;
using UnityEngine.Audio;
using Public;
using static CAudioController;
using static CSceneManager;
using static UnityEngine.PlayerPrefs;
using System.Collections;

public class CPlayer : MonoBehaviour,IDamagable_Friendly
{
    const int HP_MAX = 7, ARMOR_MAX = 6, ENERGY_MAX = 200;
    const float SKILL_CD = 20f, SKILL_TIME = 5f;
    const float ARMOR_CD = 2f, DAMAGE_CD = 1f;  //护甲回复cd,无敌时间
    private float CriticalRate { set; get; } = 0.05f;
    private float Speed { set; get; } = 6f;
    private float UpSpeed { set; get; } = 12f;
    public int HP
    {
        private set
        {
            _HP = value;
            CEventSystem.Instance.ActivateEvent(EEventType.HPChanged,_HP,HP_MAX);
        }
        get => _HP;
    }
    private int _HP;
    public int Armor
    {
        private set
        {
            _Armor = value;
            CEventSystem.Instance.ActivateEvent(EEventType.ArmorChanged,ARMOR_MAX, _Armor);
        }
        get => _Armor;
    }
    private int _Armor;
    public int Energy
    {
        private set
        {
            _Energy = value;
            CEventSystem.Instance.ActivateEvent(EEventType.EnergyChanged,ENERGY_MAX, _Energy);
        }
        get => _Energy;
    }
    private int _Energy;
    public int Coin
    {
        private set
        {
            _Coin = value;
            CEventSystem.Instance.ActivateEvent(EEventType.CoinChanged,_Coin);
        }
        get => _Coin;
    }
    private int _Coin;
    public Vector2 v_Direction;                 //移动的方向
    public float t_SpeedUp;                     //加速时间
    [SerializeField]private float t_LastAttack;                  //上次受到攻击到现在经过的时间
    private bool b_Moving=false;
    public bool b_PickPressed, b_SkillReady = true;
    public bool b_SkillPressed,b_SkillOn=false;
    float t_Armor=0;                            //下次护甲回复的时间

    public GameObject AWeapon, BWeapon;
    private Rigidbody2D m_Rigidbody;
    private Animator m_Animator;
    private GameObject pic_SkillOn, pic_SkillReady,dia_Tp;
    public AudioMixer Mixer;

    private void Start()
    {
        Initialize();
        LoadData();
    }

    private void Update()
    {
        InputCheck();
    }
    
    private void FixedUpdate()
    {
        PhysicsCheck();
        Effect();
        Move();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Room": t_LastAttack = -1f; break; //进入房间时获得无敌时间
            case "Tp":dia_Tp.SetActive(true);break;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Weapon": dia_Tp.SetActive(false); break;
            case "Tp": dia_Tp.SetActive(false); break;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Weapon":
                if(b_PickPressed)
                {
                    CAudioController.PlayAudio("fx_switch");
                    if (AWeapon.activeSelf)
                    {
                        AWeapon.SendMessage("Discard");
                        AWeapon = collision.gameObject;
                        AWeapon.SendMessage("Pick");
                    }
                    else
                    {
                        BWeapon.SendMessage("Discard");
                        BWeapon = collision.gameObject;
                        BWeapon.SendMessage("Pick");
                    }
                }
                break;
            case "Tp":
                if (Input.GetKey(KeyCode.E))
                {
                    SaveData(); NextLevel(); 
                }
                break;
        }
        if (!Input.GetKeyDown(KeyCode.E)) b_PickPressed = false;
    }

    private void Initialize()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        dia_Tp = CTool. FindFromUI("dia_tp"); 
        pic_SkillOn = CTool.FindFromUI("pic_skillon"); 
        pic_SkillReady = CTool.FindFromUI("pic_skillready");
    }

    private void InputCheck()
    {
        if(Input.GetKeyDown(KeyCode.E)) b_PickPressed=true;
        v_Direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        b_Moving = v_Direction.magnitude > 0.1f;
        if (b_Moving) v_Direction =v_Direction.normalized;
        b_SkillPressed = Input.GetMouseButton(1);   //右键使用技能
        if(Input.GetKeyDown(KeyCode.Q)&&!Input.GetMouseButton(0))   //Q键切换武器，射击时不能切换
        {
            CAudioController.PlayAudio("fx_switch");
            if (AWeapon.activeSelf)
            {
                AWeapon.SetActive(false);
                BWeapon.SetActive(true);
                BWeapon.transform.position = transform.position;
            }
            else
            {
                BWeapon.SetActive(false);
                AWeapon.SetActive(true);
                AWeapon.transform.position = transform.position;
            }
        }
    }

    private void PhysicsCheck()
    {
        if (b_Moving) transform.localScale = new Vector3(v_Direction.x < 0 ? -1 : 1, 1, 1);
        m_Animator.SetBool("run", b_Moving);
    }

    private void Move()
    {
        m_Rigidbody.velocity = v_Direction * (t_SpeedUp>0? UpSpeed:Speed);
        if (m_Rigidbody.velocity == Vector2.zero) m_Rigidbody.velocity = new Vector2(0, 0.01f);//确保进行碰撞检测
    }
    //处理技能、状态等
    private void Effect()
    {
        t_LastAttack += Time.fixedDeltaTime;
        if(t_SpeedUp>0) t_SpeedUp -= Time.fixedDeltaTime;
        if (t_Armor > 0) t_Armor -= Time.fixedDeltaTime;
        else if (Armor < ARMOR_MAX)
        {
            Armor++;
            t_Armor = ARMOR_CD;
        }

        if (b_SkillPressed&&b_SkillReady)
        {
            b_SkillOn = true;
            b_SkillReady = false;
            Invoke(nameof(EndSkillEffect), SKILL_TIME);
            Invoke(nameof(ResetSkill), SKILL_CD);
            pic_SkillOn.SetActive(true);
            pic_SkillReady.SetActive(false);
            CAudioController.PlayAudio("fx_skillon");
        }
    }
    
    private void ResetSkill()
    {
        pic_SkillReady.SetActive(true);
        b_SkillReady = true;
        CAudioController.PlayAudio("fx_skillready");
    }

    private void EndSkillEffect()
    {
        pic_SkillOn.SetActive(false);
        b_SkillOn = false;
    }
    
    public void GetDamage(int damage)
    {
        if (damage < 0) 
        {
            HP -= damage;
            if (HP > HP_MAX) HP = HP_MAX;
            return;
        }
        else if (t_LastAttack > DAMAGE_CD)
        {
            CAudioController.PlayAudio("fx_hurt");
            t_LastAttack = 0f;
            t_Armor = 4f;
            if (Armor >= damage) Armor -= damage;
            else
            {
                damage -= Armor;
                Armor = 0;
                HP -= damage;
                if(HP<= 0)
                {
                    GameObject.Find("UI").SendMessage("Mute");
                    m_Rigidbody.velocity = Vector3.zero;
                    this.enabled = false;
                    m_Animator.SetBool("dead", true);
                    Destroy(AWeapon);Destroy(BWeapon);
                    m_Rigidbody.velocity = Vector2.zero;
                    Invoke(nameof(Die), 3f);
                }
            }
        }
        if (HP < 0) HP = 0;
    }

    public void CostEnergy(int cost)
    {
        if (cost < 0) CAudioController.PlayAudio("fx_energy");
        Energy -= cost;
        if (Energy > ENERGY_MAX) Energy = ENERGY_MAX;
    }

    public void SpendCoin(int num)
    {
        if (num < 0) CAudioController.PlayAudio("fx_coin");
        Coin -= num;
    }

    private IEnumerator Die()
    {
        yield return CTool.Wait(2f);
        Destroy(this.gameObject);
        Exit();
    }

    public void SpeedUp(float time) => t_SpeedUp = time;

    public bool TellCritical() => Random.value <= CriticalRate;
    public bool TellSkillOn() => b_SkillOn;
    public int TellEnergy() => Energy;

    private void SaveData()
    {
        SetInt("Aweapon", AWeapon.name[6]-'0');
        SetInt("Bweapon", BWeapon.name[6]-'0');
        SetInt("HP", HP);
        SetInt("armor", Armor);
        SetInt("energy", Energy);
        SetInt("coin", Coin);
        SetInt("usingA", AWeapon.activeSelf ? 1 : 0);
        Mixer.GetFloat("main", out float temp); SetFloat("main",temp);
        Mixer.GetFloat("fx", out temp);SetFloat("fx", temp);
        Mixer.GetFloat("bgm", out temp);SetFloat("bgm", temp);
    }
    private void LoadData()
    {
        AWeapon = GenerateFromAsset("weapon" + GetInt("Aweapon").ToString(), transform.position, CTool.s_ZeroQuaternion); 
        BWeapon = GenerateFromAsset("weapon" + GetInt("Bweapon").ToString(), transform.position, CTool.s_ZeroQuaternion);
        if (GetInt("usingA") == 1) AWeapon.SetActive(true);
        else BWeapon.SetActive(true);
        HP = GetInt("HP");
        Armor = GetInt("armor");
        Energy = GetInt("energy");
        Coin = GetInt("coin");
        Mixer.SetFloat("main", GetFloat("main"));
        Mixer.SetFloat("bgm", GetFloat("bgm"));
        Mixer.SetFloat("fx", GetFloat("fx"));
        AWeapon.GetComponent<CWeapon>().Pick();
        BWeapon.GetComponent<CWeapon>().Pick();
    }
}