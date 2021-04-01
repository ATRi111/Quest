using UnityEngine;

//箱子用Enemy标签，但不继承enemy类
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
