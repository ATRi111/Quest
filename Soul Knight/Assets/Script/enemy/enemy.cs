using UnityEngine;
using static scene;

public class enemy : MonoBehaviour
{
    [Header("属性")]
    public string Name;
    protected float speed = 2f, speed_up = 2f;
    protected int damage = 0;//触碰伤害
    protected float cd_action = 3f, t_action = 2f;//行动间隔,行动持续时间
    protected const float cd_move = 2f, t_move = 1f;//（脱战时）随机移动的间隔，随机移动的时间
    protected float sight = 8f;//视野
    protected float speed_danmaku=5f;//发射弹幕的速度

    [Header("状态")]
    public int HP;
    protected Vector3 pos;
    public Vector2 direction = Vector2.zero;//移动或瞄准的方向
    public float angle;//移动或瞄准的角度
    bool isMoving = false;
    bool inBattle = false;//处于战斗状态
    protected bool alive;
    protected bool actionReady = true;//准备好行动
    protected bool actionIng = false;//正在行动
    protected bool moveReady = true;//(脱战时)准备好随机移动
    protected float count_action = 0f;//行动计时器
    protected bool hasAttacked;//一次行动中发射过弹幕了
    protected int num_energypoint,num_coin;//将要掉落能量点和金币的个数

    [Header("系统")]
    Rigidbody2D rb;
    protected Animator anim;
    protected GameObject player;
    protected Vector2 r_player;//该敌人到玩家的位移矢量
    protected GameObject room;//所处的房间
    public GameObject danmaku;//发射的弹幕，可能没有
    GameObject tempDanmaku;//下一发弹幕
    Rigidbody2D tempRb;//下一发弹幕的刚体
    
    protected virtual void Start()
    {
        alive = true;
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        this.enabled = false;
        Invoke(nameof(Wake), Random.Range(1f, 3f));//生成时休眠一段时间，使活动时间错开
        num_energypoint = Random.value< 0.25f ? 1 : 0;
        num_coin= Random.value < 0.25f ? 1 : 0;
    }

    void FixedUpdate()
    {
        if(alive)
        {
            PhysicsCheck();
            Act();
        }
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Player")
        {
            collision.gameObject.SendMessage("GetDamage", damage);
        }
    }

    protected void PhysicsCheck()
    {
        pos = transform.position;
        isMoving = rb.velocity.magnitude > 0.1f;
        r_player = player.transform.position - pos;
        inBattle = r_player.magnitude < sight;
        if (isMoving) transform.localScale = new Vector3(direction.x < 0 ? -1 : 1, 1, 1);
        anim.SetBool("run", isMoving);
    }

    protected virtual void Act()
    {
        if (!inBattle)
        {
            rb.velocity = direction * speed;
            if (!isMoving && moveReady)//脱战时随机移动
            {
                moveReady = false;
                direction = new Vector2(Random.Range(1f, -1f), Random.Range(1f, -1f)).normalized;//不完全随机，但更好算
                Invoke(nameof(ResetMove), cd_move);
                Invoke(nameof(EndMove), t_move);
            }
        }
        else
        {
            if (actionReady)//战斗状态下按规定的方式行动
            {
                hasAttacked = false;
                actionIng = true;
                actionReady = false;
                Invoke(nameof(EndAction), t_action);
                Invoke(nameof(ResetAction), cd_action);
            }
            else if (actionIng)
            {
                DoAct();
                count_action += Time.fixedDeltaTime;
            }
        }
    }
    protected void ResetAction() => actionReady = true;
    protected void EndAction()
    {
        count_action = 0;
        actionIng = false;
    }
    protected void ResetMove() => moveReady = true;
    protected void EndMove() => direction = Vector2.zero;
    protected virtual void DoAct()
    {
        direction = r_player.normalized;
        angle = Direction2Angle(direction);
        if (count_action < 1f) rb.velocity = direction * speed_up;
        else rb.velocity = Vector3.zero;
    }
    public void GetDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0&&alive)
        {
            alive = false;
            rb.velocity = Vector2.zero;
            GetComponent<BoxCollider2D>().enabled = false;
            if (room) room.SendMessage("EnemyDie");
            anim.SetBool("dead", true);
            Invoke(nameof(Die), 2f);
            GenerateEnergyPoint(pos, num_energypoint);
            GenerateCoinPoint(pos, num_coin);
        }
    }
    protected virtual void GenerateDanmaku() //不能发射弹幕的敌人禁止使用
    {
        direction = Angle2Direction(angle);//改变angle以改变direction
        tempDanmaku = GameObject.Instantiate(danmaku, transform.position, Quaternion.Euler(0,0,90f-angle));
        tempRb = tempDanmaku.GetComponent<Rigidbody2D>();
        tempRb.velocity = direction * speed_danmaku;
    }
    void Die()=> Destroy(this.gameObject);
    void ConnectToRoom(GameObject obj) => room = obj;//确定该敌人属于哪个房间，以确定开门的时机
    void Wake() => this.enabled=true;
}
