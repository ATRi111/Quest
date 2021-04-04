using UnityEngine;

public class weapon3 : weapon
{
    public bool testMode=false;
    public bool tightenIng;//������������
    public float tightness;//���������ĳ̶�
    const float tightness_start=0.2f;//���ҳ�ʼ�������̶�
    int t_tighten=1000;//����������Ҫ��ʱ��(����)����ʼ->100%��
    bool doubleShoot;//��Ҫ���������ӵ�
    AudioSource fx_tighten;
    protected Animator anim;

    protected override void Start()
    {
        base.Start();
        text = "����";
        cd_shoot = 500;
        cost = 2;
        deflectLevel = 0f;
        bulletOffsetDistance = 0f;
        speed_shoot = 20;
        fx_weapon = scene.FindAudio("fx_release");
        fx_tighten = scene.FindAudio("fx_weapon3");
        anim = GetComponent<Animator>();
        if (testMode) { cd_shoot = 100; cost = 0; }
    }

    protected override void Shoot()
    {
        if (this.gameObject.activeSelf)
        {
            shootPressed = Input.GetMouseButton(0);
            
            if (count_shoot > 0f) count_shoot -= _deltatime_fast;
            else if (shootPressed)
            {
                if (player.GetComponent<player>().TellEnergy() >= cost && !tightenIng)//��ʼ����
                {
                    fx_tighten.Play();
                    tightenIng = true;
                    player.GetComponent<player>().CostEnergy(cost);
                    tightness = tightness_start;
                    doubleShoot = player.GetComponent<player>().TellSkillOn();
                    //�������ɵ��ӵ�ֻ��ͼ��ʹ��
                    tempBullet = GameObject.Instantiate(bullet, transform.position, transform.localRotation);
                    tempBullet.GetComponent<Collider2D>().enabled = false;
                }
                else if (tightenIng)//����
                {
                    tightness += _deltatime_fast / (float)t_tighten;
                    tempBullet.transform.position = transform.position;
                    tempBullet.transform.rotation = transform.localRotation;
                }
            }
            else if (!shootPressed && tightenIng) Release();//����
            anim.SetBool("tight", tightenIng);
        }
    }
    void Release()
    {
        count_shoot = cd_shoot;
        tightenIng = false;
        fx_tighten.Stop();
        fx_weapon.Play();
        Destroy(tempBullet);
        GenerateBullet();
        if (doubleShoot) Invoke(nameof(GenerateBullet), 0.1f);
    }
    protected override void GenerateBullet()
    {
        base.GenerateBullet();
        float rate = (tightness > 1f ? 1f : tightness) / tightness_start;
        tempBullet.GetComponent<bullet>().SetDamageRate(rate);
    }
}
