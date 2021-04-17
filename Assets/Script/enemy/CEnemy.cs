using UnityEngine;
using Public;
using static CSceneManager;
using System.Collections;

public class CEnemy : MonoBehaviour,IDamagable
{
    public string NAME;
    protected float T_MOVE=1f, T_IDLE = 1f, T_CHASE = 1f, T_ATTACK = 1f;
    protected float VISION = 8f;                        //视野
    protected float SHOOT_SPEED = 5f;                   //发射弹幕的速度
    public int HP {protected set; get; }
    protected float Speed { set; get; } = 2f;
    protected float UpSpeed { set; get; } = 2f;

    protected int m_Damage=0;                           //触碰伤害
    protected Vector3 m_Pos;
    protected Vector2 m_Direction = Vector2.zero;       //移动或瞄准的方向
    protected float m_Angle;                            //移动或瞄准的角度
    protected Coroutine activeCoroutine;                //当前执行的行动
    private bool b_Moving = false;
    private bool b_Battling = false;                   
    protected int m_Energypoint,m_Coin;                 //将要掉落能量点和金币的个数
    
    protected Animator m_Animator;
    protected Rigidbody2D m_Rigidbody;

    public event System.Action Dead;
    protected GameObject Player;
    protected Vector2 v_Player;                         //该敌人到玩家的位移矢量
    protected CRoom m_Room;                             //所处的房间
    public GameObject Danmaku;                          //发射的弹幕
                             
    protected virtual void Start()
    {
        Player = GameObject.FindWithTag("Player");
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        m_Energypoint = Random.value < 0.25f ? 1 : 0;
        m_Coin = Random.value < 0.25f ? 1 : 0;
        StartCoroutine(Sleep(Random.Range(1f, 3f)));
    }

    private IEnumerator Sleep(float duration)
    {
        yield return CTool.Wait(duration);
        StartCoroutine(nameof(ControlledUpdate));
        StartCoroutine(nameof(Idle));
    }

    private IEnumerator ControlledUpdate()
    {
        for (; ; )
        {
            PhysicsCheck();
            yield return new WaitForFixedUpdate();
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        IDamagable_Friendly obj = collision.gameObject.GetComponent<IDamagable_Friendly>();
        if (obj != null) obj.GetDamage(m_Damage);
    }

    private void PhysicsCheck()
    {
        m_Pos = transform.position;
        b_Moving = m_Rigidbody.velocity.magnitude > 0.1f;
        v_Player = Player.transform.position - m_Pos;
        b_Battling = v_Player.magnitude < VISION;
        if (b_Moving) transform.localScale = new Vector3(m_Direction.x < 0 ? -1 : 1, 1, 1);
        m_Animator.SetBool("run", b_Moving);
    }

    protected virtual IEnumerator Wander()
    {
        m_Direction= CTool.RandomVector3().normalized;
        m_Rigidbody.velocity = m_Direction * Speed;
        yield return CTool.Wait(T_MOVE);
        StartCoroutine(Idle());
    }
    protected virtual IEnumerator Idle()
    {
        m_Rigidbody.velocity = Vector3.zero;
        yield return CTool.Wait(T_IDLE);
        if (b_Battling) StartCoroutine(Chase()); 
        else StartCoroutine(Wander());
    }
    protected virtual IEnumerator Chase()
    {
        m_Direction = v_Player.normalized;
        m_Rigidbody.velocity = m_Direction.normalized*UpSpeed;
        yield return CTool.Wait(T_CHASE);
        StartCoroutine(Attack());
    }
    protected virtual IEnumerator Attack()
    {
        m_Rigidbody.velocity = Vector2.zero;
        m_Direction = v_Player.normalized;
        m_Angle =CTool.Direction2Angle(m_Direction);
        if (Danmaku!=null) GenerateDanmaku();
        yield return CTool.Wait(T_ATTACK);
        StartCoroutine(Idle());
    }

    public void GetDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            StopAllCoroutines();
            StartCoroutine(Die());
            Dead?.Invoke();
        }
    }
    public IEnumerator Die()
    {
        m_Rigidbody.velocity = Vector2.zero;
        GetComponent<BoxCollider2D>().enabled = false;
        m_Animator.SetBool("dead", true);
        GenerateEnergyPoint(m_Pos, m_Energypoint);
        GenerateCoinPoint(m_Pos, m_Coin);
        yield return CTool.Wait(2f);
        Destroy(this.gameObject);
    }
    protected virtual void GenerateDanmaku()        //不能发射弹幕的敌人禁止使用
    {
        GameObject TempBullet;
        Rigidbody2D TempRb;
        m_Direction = CTool.Angle2Direction(m_Angle);   //每次调用此函数前改变m_Angle以改变射击方向
        TempBullet = GameObject.Instantiate(Danmaku, transform.position, Quaternion.Euler(0,0,90f-m_Angle));
        TempRb = TempBullet.GetComponent<Rigidbody2D>();
        TempRb.velocity = m_Direction * SHOOT_SPEED;
    }

    public void ConnectToRoom(CRoom room) => m_Room = room;   //确定该敌人属于哪个房间，以确定开门的时机
}
