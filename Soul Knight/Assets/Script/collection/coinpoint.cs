using UnityEngine;

public class coinpoint: MonoBehaviour
{
    const int coin = 2;
    const float distance_absorb = 5f;
    const float speed = 20f;
    Vector2 drct,r_player;
    GameObject player;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        InvokeRepeating(nameof(NewUpdate), 1f, 0.1f);//Éú³ÉÊ±ÐÝÃß1Ãë
    }

    void NewUpdate()
    {
        r_player = player.transform.position - transform.position;
        drct = r_player.magnitude > distance_absorb ? Vector2.zero : r_player.normalized;
        rb.velocity = drct * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.GetComponent<CPlayer>().SpendCoin(-coin);
            Destroy(this.gameObject);
        }
    }
}