using UnityEngine;
using static scene;

//���������ĸ���
public class weapon : MonoBehaviour
{
    [Header("����")]
    protected string text;
    protected int cd_shoot;//�����������룩
    protected int cost;//������ĵ�����
    protected int speed_shoot=20;
    public Vector3 offset = new Vector3(0.2f, -0.2f, 0f);//��������ҵ�ƫ����
    protected float bulletOffsetDistance=1f;//�ӵ����ʱ��ǹ��ƫ�ƾ���
    protected float deflectLevel=0;//����ӵ�ʱ����ƫת�ĽǶȷ�Χ

    [Header("״̬")]
    protected bool shootPressed;//Ҫ�����
    public float count_shoot=0;
    public bool isEquipped=false;//�Ƿ�װ������ ��˵��δ������
    Vector3 mousePos;//������������
    public float angle;//˳ʱ��ƫת�ĽǶ�
    protected Quaternion rotation; //��ת��
    public Vector2 direction;//��׼����ʸ��
    protected Vector3 bulletOffset;//�ӵ����ʱ��ǹ��ƫת��

    [Header("ϵͳ")]
    protected GameObject player;//ʹ����
    public GameObject bullet;//ʹ�õ��ӵ�
    protected GameObject tempBullet;//��һ���ӵ�
    protected Rigidbody2D tempRb;//��һ���ӵ��ĸ���
    GameObject UI;
    protected AudioSource fx_weapon;
    protected const int _deltatime_fast=5;//�������ʱ����(����)
    const float daltatime_fast =(float) (_deltatime_fast * 0.001);

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player");
        UI = GameObject.Find("UI");
    }

    void Update()
    {
        if(isEquipped)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    void FixedUpdate()
    {
        if (isEquipped)
        {
            Follow();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) UI.GetComponent<menu>().OpenText(text);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) UI.GetComponent<menu>().CloseText();
    }

    protected virtual void Follow()
    {
        transform.position = player.transform.position + new Vector3(offset.x*player.transform.localScale.x,offset.y,0);
        Vector3 direction = mousePos - transform.position;
        angle = Direction2Angle(direction);
        rotation = Quaternion.Euler(0f, 0f, 90f - angle);
        transform.localRotation= rotation;
        transform.localScale = new Vector3(transform.localScale.x, angle < 0f ? -1f : 1f, transform.localScale.z);
    }
    protected virtual void Shoot()
    {
        if(this.gameObject.activeSelf)
        {
            shootPressed = Input.GetMouseButton(0);
            if (count_shoot > 0f) count_shoot -= _deltatime_fast;
            else if (shootPressed && player.GetComponent<player>().TellEnergy() >= cost)
            {
                count_shoot = cd_shoot;
                player.GetComponent<player>().CostEnergy(cost);//����û��д������ʱ˫������
                fx_weapon.Play();
                GenerateBullet();
                if (player.GetComponent<player>().TellSkillOn())//���ʹ���˼��ܣ��������
                    Invoke(nameof(GenerateBullet), 0.1f); 
                    
            }
        }
    }
    public void Pick()//����������
    {
        isEquipped = true;
        GetComponent<Collider2D>().enabled = false;
        InvokeRepeating(nameof(Shoot), 0f, daltatime_fast);
    }
    public void Discard()//����������
    {
        transform.rotation = new Quaternion();
        transform.localScale = new Vector3(1, 1, 1);
        isEquipped = false;
        GetComponent<Collider2D>().enabled = true;
        CancelInvoke(nameof(Shoot));
    }
    protected virtual void GenerateBullet() //����ǵ��������������д�˺���
    {
        angle += Random.Range(-deflectLevel, deflectLevel);
        bulletOffset = new Vector3(direction.x, direction.y, 0) * bulletOffsetDistance;
        direction = Angle2Direction(angle);
        tempBullet = GameObject.Instantiate(bullet, transform.position+bulletOffset,transform.localRotation);
        tempRb = tempBullet.GetComponent<Rigidbody2D>();
        tempRb.velocity = direction * speed_shoot;
    }
    public float TellAngle() => angle;
}
