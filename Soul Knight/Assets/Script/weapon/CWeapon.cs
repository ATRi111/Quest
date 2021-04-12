using UnityEngine;
using static CSceneManager;
using Public;
using static CAudioController;

//所有武器的父类
public class CWeapon : MonoBehaviour
{
    protected string INFO;
    protected int SHOOT_CD;                                 
    protected int COST;                                     
    protected int SHOOT_SPEED=20;
    protected const int _DELTATIME = 5;         //射击检测的时间间隔(毫秒)
    const float DELTATIME = 0.001f*_DELTATIME;

    public Vector3 m_Offset = new Vector3(0.2f, -0.2f, 0f); //武器对玩家的偏移量
    protected float m_BulletOffsetDistance=1f;  //子弹射出时对枪的偏移距离
    protected float m_DeflectLevel=0;           //射出子弹时可能偏转的角度范围

    protected bool b_BhootPressed;              //要求射击
    public float t_Shoot=0;
    public bool b_Equipped=false;               //是否被拾取
    
    public float angle;                         //顺时针偏转的角度
    protected Quaternion m_Rotation;            //旋转量
    public Vector2 m_Direction;                 //瞄准方向矢量
    private Vector3 m_BulletOffset;             //子弹射出时对枪的偏转量

    private Vector3 mousePos;                   //鼠标的世界坐标
    protected GameObject Player;                //使用者
    public GameObject Bullet;                   //使用的子弹
    private GameObject UI;
    protected string fx_Weapon;
    protected GameObject TempBullet;
    protected Rigidbody2D TempRb;

    protected virtual void Start()
    {
        Player = GameObject.FindWithTag("Player");
        UI = GameObject.Find("UI");
    }

    private void Update()
    {
        if(b_Equipped)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    private void FixedUpdate()
    {
        if (b_Equipped)
        {
            Follow();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) UI.GetComponent<CMenu>().OpenText(INFO);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) UI.GetComponent<CMenu>().CloseText();
    }

    protected virtual void Follow()
    {
        transform.position = Player.transform.position + new Vector3(m_Offset.x*Player.transform.localScale.x,m_Offset.y,0);
        Vector3 direction = mousePos - transform.position;
        angle = Direction2Angle(direction);
        m_Rotation = Quaternion.Euler(0f, 0f, 90f - angle);
        transform.localRotation= m_Rotation;
        transform.localScale = new Vector3(transform.localScale.x, angle < 0f ? -1f : 1f, transform.localScale.z);
    }
    protected virtual void Shoot()
    {
        if(this.gameObject.activeSelf)
        {
            b_BhootPressed = Input.GetMouseButton(0);
            if (t_Shoot > 0f) t_Shoot -= _DELTATIME;
            else if (b_BhootPressed && Player.GetComponent<CPlayer>().TellEnergy() >= COST)
            {
                t_Shoot = SHOOT_CD;
                Player.GetComponent<CPlayer>().CostEnergy(COST);
                PlayAudio(fx_Weapon);
                GenerateBullet();
                if (Player.GetComponent<CPlayer>().TellSkillOn())//如果使用了技能，射击两次（没写双倍蓝耗）
                    Invoke(nameof(GenerateBullet), 0.1f); 
                    
            }
        }
    }
    //武器被捡起
    public void Pick()  
    {
        b_Equipped = true;
        GetComponent<Collider2D>().enabled = false;
        InvokeRepeating(nameof(Shoot), 0f, DELTATIME);
    }
    //武器被丢弃
    public void Discard()   
    {
        transform.rotation = new Quaternion();
        transform.localScale = new Vector3(1, 1, 1);
        b_Equipped = false;
        GetComponent<Collider2D>().enabled = true;
        CancelInvoke(nameof(Shoot));
    }
    //如果是单发射击，不用重写此函数
    protected virtual void GenerateBullet() 
    {
        angle += Random.Range(-m_DeflectLevel, m_DeflectLevel);
        m_BulletOffset = new Vector3(m_Direction.x, m_Direction.y, 0) * m_BulletOffsetDistance;
        m_Direction = Angle2Direction(angle);
        TempBullet = GameObject.Instantiate(Bullet, transform.position+m_BulletOffset,transform.localRotation);
        TempRb = TempBullet.GetComponent<Rigidbody2D>();
        TempRb.velocity = m_Direction * SHOOT_SPEED;
    }
    public float TellAngle() => angle;
}
