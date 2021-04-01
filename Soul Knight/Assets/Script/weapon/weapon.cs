using UnityEngine;

//���������ĸ���
public class weapon : MonoBehaviour
{
    [Header("����")]
    protected string Name;
    protected Color color;//Ʒ�ʶ�Ӧ����ɫ
    protected short cd_shoot;
    protected short cost;//������ĵ�����
    protected short speed_shoot=20;
    public Vector3 offset = new Vector3(0f, -0.2f, 0f);//��������ҵ�ƫ����
    public float bulletOffsetDistance;//�ӵ����ʱ��ǹ��ƫ�ƾ���
    protected float deflectLevel=0;//����ӵ�ʱ����ƫת�ĽǶȷ�Χ

    [Header("״̬")]
    bool shootPressed;//�Ƿ�Ҫ�����
    public short shoot_count=0;
    public bool used=false;//�Ƿ�ʹ�ã�����˵��δ������
    Vector3 mousePos;//������������
    public float angle;//˳ʱ��ƫת�ĽǶ�
    protected Quaternion rotation; //��ת��
    public Vector2 direction;//����ʸ��
    protected Vector3 bulletOffset;//�ӵ����ʱ��ǹ��ƫת��

    [Header("ϵͳ")]
    protected GameObject player;//ʹ����
    public GameObject bullet;//ʹ�õ��ӵ�
    protected const float rad = 0.0174533f;
    protected GameObject tempBullet;//��һ���ӵ�
    protected Rigidbody2D tempRb;//��һ���ӵ��ĸ���
    protected AudioSource fx_bullet;

    protected void Update()
    {
        if(used)
        {
            shootPressed = Input.GetMouseButton(0);
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    protected void FixedUpdate()
    {
        if (used)
        {
            Follow();
            Shoot();
        }
    }

    protected void Follow()
    {
        transform.position = player.transform.position + offset;
        Vector3 direction = mousePos - transform.position;
        angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        rotation = Quaternion.Euler(0f, 0f, 90f - angle);
        transform.rotation = rotation;
        transform.localScale = new Vector3(transform.localScale.x, angle < 0f ? -1f : 1f, transform.localScale.z);
    }
    void Shoot()
    {
        if (shoot_count > 0) shoot_count--;
        else if(shootPressed&& player.GetComponent<player>().TellEnergy()>=cost)
        {
            shoot_count = cd_shoot;
            angle += Random.Range(-deflectLevel, deflectLevel);
            direction = new Vector2(Mathf.Sin(angle*Mathf.Deg2Rad), Mathf.Cos(angle* Mathf.Deg2Rad));
            bulletOffset = new Vector3(direction.x, direction.y, 0) * bulletOffsetDistance;
            GenerateBullet(); 
            if (player.GetComponent<player>().TellSkillOn())//���ʹ���˼��ܣ��������
                Invoke(nameof(GenerateBullet),0.02f);   
        }
    }
    public void Pick()//����������
    {
        player = GameObject.FindWithTag("Player");
        used = true;
        GetComponent<Collider2D>().enabled = false;
    }
    public void Discard()//����������
    {
        used = false;
        GetComponent<Collider2D>().enabled = true;
    }
    protected virtual void GenerateBullet() //���ʱ���������������д�˺���
    {
        player.SendMessage("CostEnergy", cost);
        fx_bullet.Play();
        tempBullet = GameObject.Instantiate(bullet, transform.position,transform.rotation);
        tempRb = tempBullet.GetComponent<Rigidbody2D>();
        tempRb.velocity = direction * speed_shoot;
    }
}
