using UnityEngine;
using static CSceneManager;
using Public;
using static CAudioController;

//���������ĸ���
public class CWeapon : MonoBehaviour
{
    protected string INFO;
    protected int SHOOT_CD;                                 
    protected int COST;                                     
    protected int SHOOT_SPEED=20;
    protected const int _DELTATIME = 5;         //�������ʱ����(����)
    const float DELTATIME = 0.001f*_DELTATIME;

    public Vector3 m_Offset = new Vector3(0.2f, -0.2f, 0f); //��������ҵ�ƫ����
    protected float m_BulletOffsetDistance=1f;  //�ӵ����ʱ��ǹ��ƫ�ƾ���
    protected float m_DeflectLevel=0;           //����ӵ�ʱ����ƫת�ĽǶȷ�Χ

    protected bool b_BhootPressed;              //Ҫ�����
    public float t_Shoot=0;
    public bool b_Equipped=false;               //�Ƿ�ʰȡ
    
    public float angle;                         //˳ʱ��ƫת�ĽǶ�
    protected Quaternion m_Rotation;            //��ת��
    public Vector2 m_Direction;                 //��׼����ʸ��
    private Vector3 m_BulletOffset;             //�ӵ����ʱ��ǹ��ƫת��

    private Vector3 mousePos;                   //������������
    protected GameObject Player;                //ʹ����
    public GameObject Bullet;                   //ʹ�õ��ӵ�
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
                if (Player.GetComponent<CPlayer>().TellSkillOn())//���ʹ���˼��ܣ�������Σ�ûд˫�����ģ�
                    Invoke(nameof(GenerateBullet), 0.1f); 
                    
            }
        }
    }
    //����������
    public void Pick()  
    {
        b_Equipped = true;
        GetComponent<Collider2D>().enabled = false;
        InvokeRepeating(nameof(Shoot), 0f, DELTATIME);
    }
    //����������
    public void Discard()   
    {
        transform.rotation = new Quaternion();
        transform.localScale = new Vector3(1, 1, 1);
        b_Equipped = false;
        GetComponent<Collider2D>().enabled = true;
        CancelInvoke(nameof(Shoot));
    }
    //����ǵ��������������д�˺���
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
