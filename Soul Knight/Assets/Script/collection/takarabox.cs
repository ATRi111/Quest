using UnityEngine;
using static scene;

public class takarabox : MonoBehaviour
{
    AudioSource fx_open;
    bool containsWeapon;
    int weaponID=0,num_coin,num_energy;
    const int maxID = 5;
    GameObject weapon;
    void Start()
    {
        fx_open = FindAudio("fx_open");
        containsWeapon = Random.value < 0.5f;
        if (containsWeapon)
        {
            weaponID = Random.Range(2, maxID + 1);
            weapon = Instantiate(FindFromAsset("weapon" + weaponID.ToString()), transform.position, zeroQuaternion);
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
