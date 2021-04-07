using UnityEngine;

//所有子弹的父类
public class bullet : MonoBehaviour
{
    public int damage;
    protected int criticalDamage;//暴击伤害
    protected bool penetrating=false;//有穿透性的
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
