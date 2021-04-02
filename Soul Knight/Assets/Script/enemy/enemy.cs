using UnityEngine;

public class enemy : MonoBehaviour
{
    [Header("����")]
    public string Name;
    protected float speed = 2f, speed_up = 3f;
    protected short damage = 0;//�����˺�
    protected float cd_action = 3f, t_action = 2f;//�ж����,�ж�����ʱ��
    protected const float cd_move = 2f, t_move = 1f;//����սʱ������ƶ��ļ��������ƶ���ʱ��

    [Header("״̬")]
    public short HP;
    public float rad;
    public Vector2 drct = Vector2.zero;//�ƶ��ķ���
    public bool isMoving = false;
    public bool inBattle = false;//����ս��״̬
    public bool actionReady = true;//׼�����ж�
    public bool actionIng = false;//�����ж�
    public bool moveReady = true;//(��սʱ)׼��������ƶ�
    public float count_action = 0f;//�ж���ʱ��

    [Header("ϵͳ")]
    Rigidbody2D rb;
    protected Animator anim;
    protected GameObject player;
    protected Vector2 r_player;//�õ��˵���ҵ�λ��ʸ��
    protected GameObject room;//�����ķ���
    public short counter;

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        this.enabled = false;
        Invoke(nameof(Wake), Random.Range(1f, 3f));//�ʱ���
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
            if (!isMoving && moveReady)//��սʱ����ƶ�
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
            if (actionReady)//ս��״̬�°��涨�ķ�ʽ�ж�
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
        drct = count_action > 1f ? r_player.normalized : Vector2.zero;// ��������ڵķ����ƶ�
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
    void ConnectToRoom(GameObject obj) => room = obj;//ȷ���õ��������ĸ����䣬��ȷ�����ŵ�ʱ��
    void Wake() => this.enabled=true;
}
