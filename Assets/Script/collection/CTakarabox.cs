using UnityEngine;
using Public;
using static CSceneManager;
using static CAudioController;

public class CTakarabox : MonoBehaviour
{
    bool b_ContainWeapon;
    int m_CoinNum,m_EnergyNum;
    private GameObject m_Weapon;

    private void Start()
    {
        //控制不同武器出现概率
        static string RandomID()
        {
            int num = Random.Range(0, 51);
            if (num < 21) return "1";
            else if (num < 31) return "2";
            else if (num < 41) return "4";
            else if (num < 51) return "5";
            else return "0";
        }
        b_ContainWeapon = Random.value < 0.5f;
        if (b_ContainWeapon)
        {
            m_Weapon = GenerateFromAsset("weapon" + RandomID(), transform.position, CTool.s_ZeroQuaternion); 
        }
        else
        {
            m_EnergyNum = Random.Range(2, 5);
            m_CoinNum = Random.Range(4, 7);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (!b_ContainWeapon)
            {
                GenerateEnergyPoint(transform.position, m_EnergyNum);
                GenerateCoinPoint(transform.position,m_CoinNum);
            }
            else m_Weapon.SetActive(true);
            CAudioController.PlayAudio("fx_open");
            Destroy(this.gameObject);
        }
    }

}
