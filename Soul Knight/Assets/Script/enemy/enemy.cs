using UnityEngine;
using static scene;

public class enemy : MonoBehaviour
{
    [Header("����")]
    public string Name;
    protected float speed = 2f, speed_up = 2f;
    protected int damage = 0;//�����˺�
    protected float cd_action = 3f, t_action = 2f;//�ж����,�ж�����ʱ��
    protected const float cd_move = 2f, t_move = 1f;//����սʱ������ƶ��ļ��������ƶ���ʱ��
    protected float sight = 8f;//��Ұ
    protected float speed_danmaku=5f;//���䵯Ļ���ٶ�

    [Header("״̬")]
    public int HP;
    protected Vector3 pos;
    public Vector2 direction = Vector2.zero;//�ƶ�����׼�ķ���
    public float angle;//�ƶ�����׼�ĽǶ�
    bool isMoving = false;
    bool inBattle = false;//����ս��״̬
    protected bool alive;
    protected bool actionReady = true;//׼�����ж�
    protected bool actionIng = false;//�����ж�
    protected bool moveReady = true;//(��սʱ)׼��������ƶ�
    protected float count_action = 0f;//�ж���ʱ��
    protected bool hasAttacked;//һ���ж��з������Ļ��
    protected int num_energypoint,num_coin;//��Ҫ����������ͽ�ҵĸ���

    [Header("ϵͳ")]
    Rigidbody2D rb;
    protected Animator anim;
    protected GameObject player;
    protected Vector2 r_player;//�õ��˵���ҵ�λ��ʸ��
    protected GameObject room;//�����ķ���
    public GameObject danmaku;//����ĵ�Ļ������û��
    GameObject tempDanmaku;//��һ����Ļ
    Rigidbody2D tempRb;//��һ����Ļ�ĸ���
    
    protected virtual void Start()
    {
        alive = true;
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        this.enabled = false;
        Invoke(nameof(Wake), Random.Range(1f, 3f));//����ʱ����һ��ʱ�䣬ʹ�ʱ���
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
            if (!isMoving && moveReady)//��սʱ����ƶ�
            {
                moveReady = false;
                direction = new Vector2(Random.Range(1f, -1f), Random.Range(1f, -1f)).normalized;//����ȫ�������������
                Invoke(nameof(ResetMove), cd_move);
                Invoke(nameof(EndMove), t_move);
            }
        }
        else
        {
            if (actionReady)//ս��״̬�°��涨�ķ�ʽ�ж�
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
    protected virtual void GenerateDanmaku() //���ܷ��䵯Ļ�ĵ��˽�ֹʹ��
    {
        direction = Angle2Direction(angle);//�ı�angle�Ըı�direction
        tempDanmaku = GameObject.Instantiate(danmaku, transform.position, Quaternion.Euler(0,0,90f-angle));
        tempRb = tempDanmaku.GetComponent<Rigidbody2D>();
        tempRb.velocity = direction * speed_danmaku;
    }
    void Die()=> Destroy(this.gameObject);
    void ConnectToRoom(GameObject obj) => room = obj;//ȷ���õ��������ĸ����䣬��ȷ�����ŵ�ʱ��
    void Wake() => this.enabled=true;
}
