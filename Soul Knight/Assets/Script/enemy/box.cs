using UnityEngine;

//������Enemy��ǩ�������̳�enemy��
public class box : MonoBehaviour
{
    public short HP=4;
    AudioSource fx_broke;

    void Start()
    {
        fx_broke = scene.FindAudio("fx_broke");
    }

    public void GetDamage(short damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            fx_broke.Play();
            Destroy(this.gameObject);
        }
    }
}
