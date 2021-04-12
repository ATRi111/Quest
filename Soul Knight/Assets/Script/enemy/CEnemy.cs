using UnityEngine;
using Public;
using static CSceneManager;

public class CEnemy : MonoBehaviour,IDamagable
{
    public string NAME;
    protected float ACTION_CD = 3f, ACTION_TIME = 2f;   //�ж����,�ж�����ʱ��
    protected const float MOVE_CD = 2f, MOVE_TIME = 1f; //����սʱ������ƶ��ļ��������ƶ���ʱ��
    protected float VISION = 8f;                        //��Ұ
    protected float SHOOT_SPEED = 5f;                   //���䵯Ļ���ٶ�
    public int HP { set; get; }
    protected int Damage { set; get; } = 0;             //�����˺�
    protected float Speed { set; get; } = 2f;
    protected float UpSpeed { set; get; } = 2f;

    protected Vector3 m_Pos;
    public Vector2 m_Direction = Vector2.zero;          //�ƶ�����׼�ķ���
    public float m_Angle;                               //�ƶ�����׼�ĽǶ�
    private bool b_Moving = false;
    private bool b_Battling = false;                   
    protected bool b_Alive=true;
    protected bool b_ActionReady = true,b_ActionIng = false;
    protected bool b_MoveReady = true;                  //(��սʱ)׼��������ƶ�
    protected float t_Action = 0f;
    protected bool b_HasAttacked;                       //һ���ж��й�������
    protected int m_Energypoint,m_Coin;                 //��Ҫ����������ͽ�ҵĸ���
    
    protected Animator m_Animator;
    protected Rigidbody2D m_Rigidbody;
    protected GameObject Player;
    protected Vector2 v_Player;                         //�õ��˵���ҵ�λ��ʸ��
    protected GameObject Room;                          //�����ķ���
    public GameObject Danmaku;                          //����ĵ�Ļ
                             
    protected virtual void Start()
    {
        Player = GameObject.FindWithTag("Player");
        m_Rigidbody = GetComponent<Rigidbody2D>();
        m_Animator = GetComponent<Animator>();
        this.enabled = false;
        Invoke(nameof(Wake), Random.Range(1f, 3f));//����ʱ����һ��ʱ�䣬ʹ�ʱ���
        m_Energypoint = Random.value< 0.25f ? 1 : 0;
        m_Coin= Random.value < 0.25f ? 1 : 0;
    }
    private void Wake() => this.enabled = true;

    private void FixedUpdate()
    {
        if(b_Alive)
        {
            PhysicsCheck();
            Act();
        }
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("GetDamage", Damage);
        }
    }

    protected void PhysicsCheck()
    {
        m_Pos = transform.position;
        b_Moving = m_Rigidbody.velocity.magnitude > 0.1f;
        v_Player = Player.transform.position - m_Pos;
        b_Battling = v_Player.magnitude < VISION;
        if (b_Moving) transform.localScale = new Vector3(m_Direction.x < 0 ? -1 : 1, 1, 1);
        m_Animator.SetBool("run", b_Moving);
    }

    protected virtual void Act()
    {
        if (!b_Battling)
        {
            m_Rigidbody.velocity = m_Direction * Speed;
            if (!b_Moving && b_MoveReady)   //��սʱ����ƶ�
            {
                b_MoveReady = false;
                m_Direction = new Vector2(Random.Range(1f, -1f), Random.Range(1f, -1f)).normalized;//����ȫ�������������
                Invoke(nameof(ResetMove), MOVE_CD);
                Invoke(nameof(EndMove), MOVE_TIME);
            }
        }
        else
        {
            if (b_ActionReady)              //ս��״̬�°��涨�ķ�ʽ�ж�
            {
                b_HasAttacked = false;
                b_ActionIng = true;
                b_ActionReady = false;
                Invoke(nameof(EndAction), ACTION_TIME);
                Invoke(nameof(ResetAction), ACTION_CD);
            }
            else if (b_ActionIng)
            {
                DoAct();
                t_Action += Time.fixedDeltaTime;
            }
        }
    }
    protected void ResetAction() => b_ActionReady = true;
    protected void EndAction()
    {
        t_Action = 0;
        b_ActionIng = false;
    }
    protected void ResetMove() => b_MoveReady = true;
    protected void EndMove() => m_Direction = Vector2.zero;
    protected virtual void DoAct()
    {
        m_Direction = v_Player.normalized;
        m_Angle = Direction2Angle(m_Direction);
        if (t_Action < 1f) m_Rigidbody.velocity = m_Direction * UpSpeed;
        else m_Rigidbody.velocity = Vector3.zero;
    }

    public void GetDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0&&b_Alive)
        {
            b_Alive = false;
            m_Rigidbody.velocity = Vector2.zero;
            GetComponent<BoxCollider2D>().enabled = false;
            if (Room) Room.SendMessage("EnemyDie");
            m_Animator.SetBool("dead", true);
            Invoke(nameof(Die), 2f);
            GenerateEnergyPoint(m_Pos, m_Energypoint);
            GenerateCoinPoint(m_Pos, m_Coin);
        }
    }
    public void Die() => Destroy(this.gameObject);
    protected virtual void GenerateDanmaku()        //���ܷ��䵯Ļ�ĵ��˽�ֹʹ��
    {
        GameObject TempBullet;
        Rigidbody2D TempRb;
        m_Direction = Angle2Direction(m_Angle);     //�ı�angle�Ըı�direction
        TempBullet = GameObject.Instantiate(Danmaku, transform.position, Quaternion.Euler(0,0,90f-m_Angle));
        TempRb = TempBullet.GetComponent<Rigidbody2D>();
        TempRb.velocity = m_Direction * SHOOT_SPEED;
    }

    void ConnectToRoom(GameObject obj) => Room = obj;   //ȷ���õ��������ĸ����䣬��ȷ�����ŵ�ʱ��
}
