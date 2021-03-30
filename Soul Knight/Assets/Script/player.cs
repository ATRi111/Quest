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
    const int maxHP=7;
    const int maxArmor =6;
    const int maxEnergy =200;
    float criticalRate=0.05f;//暴击率
    float speed = 5f;
    const int skill_cd =1000;//技能冷却（以deltatime的倍数计）
    const int skilleffect_t =200;//技能效果时间

    [Header("状态")]
    public int HP;
    public int armor;
    public int energy;
    public Vector2 pos;
    public Vector2 drct;//移动的方向
    public bool isMoving=false;
    bool usingA=true;//是否在使用武器A
    bool skillPressed;//要求使用技能
    public int skill_count=0;
    public int skilleffect_count=0;

    [Header("系统")]
    Rigidbody2D rb;
    BoxCollider2D coll;
    Animator anim;
    public GameObject Aweapon;
    public GameObject Bweapon;
    public GameObject skill_picture1, skill_picture2;
    void Start()
    {
        Initialize();
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
        if (collision.CompareTag("Weapon")&&Input.GetButtonDown("Pick"))
        {
            if(usingA)
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
    }
    
    void Initialize()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        HP = maxHP;
        armor = maxArmor;
        energy = maxEnergy;
        Aweapon.SendMessage("Pick");
    }
    void InputCheck()
    {
        drct = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        isMoving = drct.magnitude > 0;
        if (isMoving) drct =drct.normalized;
        skillPressed = Input.GetMouseButton(1);//右键使用技能
    }
    void PhysicsCheck()
    {
        pos = transform.position;
    }
    void Move()
    {
        if (isMoving) transform.localScale = new Vector3(drct.x < 0 ? -1 : 1, 1, 1);
        rb.velocity = drct * speed;
        anim.SetBool("run", isMoving);
    }
    void Effect()//处理技能、状态等
    {
        if (skill_count > 0) skill_count--;
        else if(skillPressed)
        {
            skill_count = skill_cd;
            skilleffect_count = skilleffect_t;

        }
        if (skilleffect_count > 0) skilleffect_count--;
        skill_picture1.SetActive(skilleffect_count > 0);
        skill_picture2.SetActive(skilleffect_count == 0 && skill_count == 0);
    }
    public bool IsCritical() => Random.value <= criticalRate;
    public bool IsUsingSkill() => skilleffect_count > 0;
    public int tellEnergy() => energy;
    public void CostEnergy(int cost)
    {
        energy -= cost;
    }
}
