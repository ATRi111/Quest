using UnityEngine;
using static scene;

public class box : MonoBehaviour
{
    public int HP=4;
    AudioSource fx_broke;
    bool energypoint;//将会掉落一个能量点

    void Start()
    {
        fx_broke = FindAudio("fx_broke");
        energypoint = Random.value < 0.125f;
    }

    public void GetDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            if (energypoint) GenerateEnergyPoint(transform.position);
            fx_broke.Play();
            Destroy(this.gameObject);
        }
    }
}
