using UnityEngine;

//�����ӵ��ĸ���
public class bullet : MonoBehaviour
{
    public int damage;
    protected int criticalDamage;//�����˺�
    protected bool penetrating=false;//�д�͸�Ե�
    protected float damagerate = 1.0001f;
    protected void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Wall": case "Door": case "Obstacle":Destroy(this.gameObject); break;
            case "Enemy":case "Box":
                if (GameObject.Find("knight").GetComponent<player>().TellCritical()) damage =criticalDamage;
                collision.gameObject.SendMessage("GetDamage", (int)(damage*damagerate));
                if (!penetrating) Destroy(this.gameObject); 
                    break;
        }    
    }
    public void SetDamageRate(float rate) => damagerate = rate;
}
