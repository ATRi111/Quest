using UnityEngine;

//���������ĸ���
public class weapon : MonoBehaviour
{
    [Header("����")]
    protected string Name;
    protected int cd_shoot;//�����������룩
    protected int cost;//������ĵ�����
    protected int speed_shoot=20;
    public Vector3 offset = new Vector3(0.2f, -0.2f, 0f);//��������ҵ�ƫ����
    public float bulletOffsetDistance=0.5f;//�ӵ����ʱ��ǹ��ƫ�ƾ���
    protected float deflectLevel=0;//����ӵ�ʱ����ƫת�ĽǶȷ�Χ

    [Header("״̬")]
    bool shootPressed;//Ҫ�����
    public float count_shoot=0;
    public bool isEquipped=false;//�Ƿ�װ������ ��˵��δ������
    Vector3 mousePos;//������������
    public float angle;//˳ʱ��ƫת�ĽǶ�
    protected Quaternion rotation; //��ת��
    public Vector2 direction;//����ʸ��
    protected Vector3 bulletOffset;//�ӵ����ʱ��ǹ��ƫת��

    [Header("ϵͳ")]
    GameObject player;//ʹ����
    public GameObject bullet;//ʹ�õ��ӵ�
    GameObject tempBullet;//��һ���ӵ�
    Rigidbody2D tempRb;//��һ���ӵ��ĸ���
    public AudioSource fx_weapon;
    const int _deltatime_fast=5;//�������ʱ����(����)
    const float daltatime_fast =(float) (_deltatime_fast * 0.001);
    public int counter;

    protected virtual void Start()
    {
        player = GameObject.FindWithTag("Player");
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

    protected void Follow()
    {
        transform.position = player.transform.position + new Vector3(offset.x*player.transform.localScale.x,offset.y,0);
        Vector3 direction = mousePos - transform.position;
        angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        rotation = Quaternion.Euler(0f, 0f, 90f - angle);
        transform.rotation = rotation;
        transform.localScale = new Vector3(transform.localScale.x, angle < 0f ? -1f : 1f, transform.localScale.z);
    }
    public void Shoot()
    {
        if(this.gameObject.activeSelf)
        {
            shootPressed = Input.GetMouseButton(0);
            if (count_shoot > 0f) count_shoot -= _deltatime_fast;
            else if (shootPressed && player.GetComponent<player>().TellEnergy() >= cost)
            {
                count_shoot = cd_shoot;
                player.GetComponent<player>().CostEnergy(cost);

                angle += Random.Range(-deflectLevel, deflectLevel);
                bulletOffset = new Vector3(direction.x, direction.y, 0) * bulletOffsetDistance;
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
        direction = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));//�ı�angle�Ըı�direction
        tempBullet = GameObject.Instantiate(bullet, transform.position+bulletOffset,transform.rotation);
        tempRb = tempBullet.GetComponent<Rigidbody2D>();
        tempRb.velocity = direction * speed_shoot;
        fx_weapon.Play();
    }
}
