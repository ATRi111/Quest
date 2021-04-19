using UnityEngine;
using static CAudioController;

public class CBow : CWeapon
{
    protected float TIGHTNESS_START;//弓弦初始的拉紧程度

    public bool b_Tightning;        //正在拉紧弓弦
    public float m_Tightness;       //弓弦拉紧的程度
    protected int t_Tighten;        //拉紧弓弦需要的时间(毫秒)（初始->100%）
    protected bool b_DoubleShoot;             //将要发射两发子弹
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
                if (Player.GetComponent<CPlayer>().TellEnergy() >= COST && !b_Tightning)    //开始蓄力
                {
                    
                    b_Tightning = true;
                    Player.GetComponent<CPlayer>().CostEnergy(COST);
                    m_Tightness = TIGHTNESS_START;
                    b_DoubleShoot = Player.GetComponent<CPlayer>().TellSkillOn();
                    //这里生成的子弹只作图像使用
                    TempBullet = GameObject.Instantiate(Bullet, transform.position, transform.localRotation);
                    TempBullet.GetComponent<Collider2D>().enabled = false;
                }
                else if (b_Tightning)   //蓄力
                {
                    m_Tightness += _DELTATIME / (float)t_Tighten;
                    TempBullet.transform.position = transform.position;
                    TempBullet.transform.rotation = transform.localRotation;
                }
            }
            else if (!b_ShootPressed && b_Tightning) Release(); //发射
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
