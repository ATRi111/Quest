using UnityEngine;
using static CAudioController;

public class CBow : CWeapon
{
    protected float TIGHTNESS_START;//���ҳ�ʼ�������̶�

    public bool b_Tightning;        //������������
    public float m_Tightness;       //���������ĳ̶�
    protected int t_Tighten;        //����������Ҫ��ʱ��(����)����ʼ->100%��
    protected bool b_DoubleShoot;             //��Ҫ���������ӵ�
    protected string fx_Tighten;
    protected Animator m_Animator;

    protected override void Start()
    {
        base.Start();
        m_Animator = GetComponent<Animator>();
    }

    protected override void Shoot()
    {
        if (this.gameObject.activeSelf)
        {
            b_ShootPressed = Input.GetMouseButton(0);

            if (t_Shoot > 0f) t_Shoot -= _DELTATIME;
            else if (b_ShootPressed)
            {
                if (Player.GetComponent<CPlayer>().TellEnergy() >= COST && !b_Tightning)    //��ʼ����
                {
                    
                    b_Tightning = true;
                    Player.GetComponent<CPlayer>().CostEnergy(COST);
                    m_Tightness = TIGHTNESS_START;
                    b_DoubleShoot = Player.GetComponent<CPlayer>().TellSkillOn();
                    //�������ɵ��ӵ�ֻ��ͼ��ʹ��
                    TempBullet = GameObject.Instantiate(Bullet, transform.position, transform.localRotation);
                    TempBullet.GetComponent<Collider2D>().enabled = false;
                }
                else if (b_Tightning)   //����
                {
                    m_Tightness += _DELTATIME / (float)t_Tighten;
                    TempBullet.transform.position = transform.position;
                    TempBullet.transform.rotation = transform.localRotation;
                }
            }
            else if (!b_ShootPressed && b_Tightning) Release(); //����
            m_Animator.SetBool("tight", b_Tightning);
        }
    }
    protected virtual void Release()
    {
        t_Shoot = SHOOT_CD;
        b_Tightning = false;
        StopAudio(fx_Tighten);
        PlayAudio(fx_Weapon);
        Destroy(TempBullet);
        GenerateBullet();
        if (b_DoubleShoot) Invoke(nameof(GenerateBullet), 0.1f);
    }
    protected override void GenerateBullet()
    {
        base.GenerateBullet();
        float rate = (m_Tightness > 1f ? 1f : m_Tightness) / TIGHTNESS_START;
        TempBullet.GetComponent<CBullet>().SetDamageRate(rate);
    }
}
