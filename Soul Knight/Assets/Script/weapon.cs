using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���������ĸ���
public class weapon : MonoBehaviour
{
    [Header("����")]
    protected string Name;
    protected Color color;//Ʒ�ʶ�Ӧ����ɫ
    protected int shoot_cd;
    protected int cost;//������ĵ�����
    public Vector3 offset = new Vector3(0f, -0.2f, 0f);//��������ҵ�ƫ����
    public float bulletOffsetDistance;//�ӵ����ʱ��ǹ��ƫ�ƾ���

    [Header("״̬")]
    public bool shootPressed;//�Ƿ�Ҫ�����
    public int shoot_count=0;
    public bool used=false;//�Ƿ�ʹ�ã��� ��˵��δ������
    Vector3 mousePos;//������������
    public float angle;//˳ʱ��ƫת�ĽǶ�
    protected Quaternion rotation; //��ת��
    public Vector2 direction;//����ʸ��
    protected Vector3 bulletOffset;//�ӵ����ʱ��ǹ��ƫת��

    [Header("ϵͳ")]
    public GameObject user;//ʹ����
    public GameObject bullet;//ʹ�õ��ӵ�
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
            if (user.GetComponent<player>().IsUsingSkill())//���ʹ���˼��ܣ��������
            { 
                Invoke(nameof(GenerateBullet),0.02f); 
                user.SendMessage("CostEnergy", cost);
            }
        }
    }
    public void Pick()//����������
    {
        used = true;
    }
    public void Discard()//����������
    {
        used = false;
    }
    protected virtual void GenerateBullet() {}
}
