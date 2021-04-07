using UnityEngine;
using static scene;

public class takarabox : MonoBehaviour
{
    AudioSource fx_open;
    bool containsWeapon;
    int num_coin,num_energy;
    const int maxID = 5;
    GameObject weapon;
    void Start()
    {
        string RandomID()//控制不同武器出现概率
        {
            int num = Random.Range(0, 51);
            if (num < 21) return "1";
            else if (num < 31) return "2";
            else if (num < 41) return "4";
            else if (num < 51) return "5";
            else return "0";
        }

        fx_open = FindAudio("fx_open");
        containsWeapon = Random.value < 0.5f;
        if (containsWeapon)
        {
            weapon = GenerateFromAsset("weapon" + RandomID(), transform.position, zeroQuaternion);
        }
        else
        {
            num_energy = Random.Range(2, 5);
            num_coin = Random.Range(4, 7);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (!containsWeapon)
            {
                GenerateEnergyPoint(transform.position, num_energy);
                GenerateCoinPoint(transform.position,num_coin);
            }
            else weapon.SetActive(true);
            fx_open.Play();
            Destroy(this.gameObject);
        }
    }

}
