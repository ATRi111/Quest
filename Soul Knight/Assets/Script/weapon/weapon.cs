using UnityEngine;

//所有武器的父类
public class weapon : MonoBehaviour
{
    [Header("属性")]
    protected string Name;
    protected int cd_shoot;//射击间隔（毫秒）
    protected int cost;//射击消耗的能量
    protected int speed_shoot=20;
    public Vector3 offset = new Vector3(0.2f, -0.2f, 0f);//武器对玩家的偏移量
    public float bulletOffsetDistance=0.5f;//子弹射出时对枪的偏移距离
    protected float deflectLevel=0;//射出子弹时可能偏转的角度范围

    [Header("状态")]
    bool shootPressed;//要求射击
    public float count_shoot=0;
    public bool isEquipped=false;//是否被装备，否 则说明未被捡起
    Vector3 mousePos;//鼠标的世界坐标
    public float angle;//顺时针偏转的角度
    protected Quaternion rotation; //旋转量
    public Vector2 direction;//方向矢量
    protected Vector3 bulletOffset;//子弹射出时对枪的偏转量

    [Header("系统")]
    GameObject player;//使用者
    public GameObject bullet;//使用的子弹
    GameObject tempBullet;//下一发子弹
    Rigidbody2D tempRb;//下一发子弹的刚体
    public AudioSource fx_weapon;
    const int _deltatime_fast=5;//射击检测的时间间隔(毫秒)
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
                if (player.GetComponent<player>().TellSkillOn())//如果使用了技能，射击两次
                    Invoke(nameof(GenerateBullet), 0.1f);
            }
        }
    }
    public void Pick()//武器被捡起
    {
        isEquipped = true;
        GetComponent<Collider2D>().enabled = false;
        InvokeRepeating(nameof(Shoot), 0f, daltatime_fast);
    }
    public void Discard()//武器被丢弃
    {
        transform.rotation = new Quaternion();
        transform.localScale = new Vector3(1, 1, 1);
        isEquipped = false;
        GetComponent<Collider2D>().enabled = true;
        CancelInvoke(nameof(Shoot));
    }
    protected virtual void GenerateBullet() //如果是单发射击，不用重写此函数
    {
        direction = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));//改变angle以改变direction
        tempBullet = GameObject.Instantiate(bullet, transform.position+bulletOffset,transform.rotation);
        tempRb = tempBullet.GetComponent<Rigidbody2D>();
        tempRb.velocity = direction * speed_shoot;
        fx_weapon.Play();
    }
}
