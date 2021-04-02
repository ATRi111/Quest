using UnityEngine;

public class enemy : MonoBehaviour
{
    [Header("属性")]
    public string Name;
    protected float speed = 2f, speed_up = 3f;
    protected short damage = 0;//触碰伤害
    protected float cd_action = 3f, t_action = 2f;//行动间隔,行动持续时间
    protected const float cd_move = 2f, t_move = 1f;//（脱战时）随机移动的间隔，随机移动的时间

    [Header("状态")]
    public short HP;
    public float rad;
    public Vector2 drct = Vector2.zero;//移动的方向
    public bool isMoving = false;
    public bool inBattle = false;//处于战斗状态
    public bool actionReady = true;//准备好行动
    public bool actionIng = false;//正在行动
    public bool moveReady = true;//(脱战时)准备好随机移动
    public float count_action = 0f;//行动计时器

    [Header("系统")]
    Rigidbody2D rb;
    protected Animator anim;
    protected GameObject player;
    protected Vector2 r_player;//该敌人到玩家的位移矢量
    protected GameObject room;//所处的房间
    public short counter;

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        this.enabled = false;
        Invoke(nameof(Wake), Random.Range(1f, 3f));//活动时间错开
    }

    void FixedUpdate()
    {
        PhysicsCheck();
        Act();
    }

    protected void PhysicsCheck()
    {
        isMoving = rb.velocity.magnitude > 0.1f;
        r_player = player.transform.position - transform.position;
        inBattle = r_player.magnitude < 8f;
        if (isMoving) transform.localScale = new Vector3(drct.x < 0 ? -1 : 1, 1, 1);
        anim.SetBool("run", isMoving);
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) collision.gameObject.SendMessage("GetDamage", damage);
    }

    void Act()
    {
        if (!inBattle)
        {
            rb.velocity = drct * speed;
            if (!isMoving && moveReady)//脱战时随机移动
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
            if (actionReady)//战斗状态下按规定的方式行动
            {
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
    public void GetDamage(short damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            HP = 1000;
            speed = speed_up = 0;rb.velocity = Vector2.zero;
            GetComponent<BoxCollider2D>().enabled = false;

            Invoke(nameof(Die), 2f);
            if (room) room.SendMessage("EnemyDie");
            anim.SetBool("dead", true);
        }
    }
    protected virtual void DoAct()
    {
        drct = count_action > 1f ? r_player.normalized : Vector2.zero;// 向玩家所在的方向移动
        rb.velocity = drct * speed_up;
    }
    void Die()
    {
        Destroy(this.gameObject);
    }
    void ResetAction() => actionReady = true;
    void EndAction()
    {
        count_action = 0;
        actionIng = false;
    }
    void ResetMove() => moveReady = true;
    void EndMove() => drct = Vector2.zero;
    void ConnectToRoom(GameObject obj) => room = obj;//确定该敌人属于哪个房间，以确定开门的时机
    void Wake() => this.enabled=true;
}
