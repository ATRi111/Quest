using UnityEngine;

//�����ӵ��ĸ���
public class bullet : MonoBehaviour
{
    protected int damage;
    protected int criticalDamage;//�����˺�

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Wall": case "Door": case "Obstacle": Destroy(this.gameObject); break;
            case "Enemy":case "Box":
                if (GameObject.Find("knight").GetComponent<player>().TellCritical()) damage =criticalDamage;
                collision.gameObject.SendMessage("GetDamage", damage);
                Destroy(this.gameObject); break;
        }    
    }
}
