using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//所有武器的父类
public class weapon : MonoBehaviour
{
    [Header("属性")]
    protected string Name;
    protected Color color;//品质对应的颜色
    protected int shoot_cd;
    protected int cost;//射击消耗的能量
    public Vector3 offset = new Vector3(0f, -0.2f, 0f);//武器对玩家的偏移量
    public float bulletOffsetDistance;//子弹射出时对枪的偏移距离

    [Header("状态")]
    public bool shootPressed;//是否要求射击
    public int shoot_count=0;
    public bool used=false;//是否被使用，否 则说明未被捡起
    Vector3 mousePos;//鼠标的世界坐标
    public float angle;//顺时针偏转的角度
    protected Quaternion rotation; //旋转量
    public Vector2 direction;//方向矢量
    protected Vector3 bulletOffset;//子弹射出时对枪的偏转量

    [Header("系统")]
    public GameObject user;//使用者
    public GameObject bullet;//使用的子弹
    protected const float rad = 0.0174533f;

    void Start()
    {
       
    }

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
            Move();
            Shoot();
        }
    }

    protected void Move()
    {
        transform.position = user.transform.position + offset;
        Vector3 direction = mousePos - transform.position;
        angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        rotation = Quaternion.Euler(0f, 0f, 90f - angle);
        transform.rotation = rotation;
        transform.localScale = new Vector3(transform.localScale.x, angle < 0f ? -1f : 1f, transform.localScale.z);
    }
    void Shoot()
    {
        if (shoot_count > 0) shoot_count--;
        else if(shootPressed&& user.GetComponent<player>().tellEnergy()>=cost)
        {
            shoot_count = shoot_cd;
            direction = new Vector2(Mathf.Sin(angle*rad), Mathf.Cos(angle*rad));
            bulletOffset = new Vector3(direction.x, direction.y, 0) * bulletOffsetDistance;
            GenerateBullet(); user.SendMessage("CostEnergy", cost);
            if (user.GetComponent<player>().IsUsingSkill())//如果使用了技能，射击两次
            { 
                Invoke(nameof(GenerateBullet),0.02f); 
                user.SendMessage("CostEnergy", cost);
            }
        }
    }
    public void Pick()//武器被捡起
    {
        used = true;
    }
    public void Discard()//武器被丢弃
    {
        used = false;
    }
    protected virtual void GenerateBullet() {}
}
