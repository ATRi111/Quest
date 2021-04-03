using UnityEngine;

public class energypoint : MonoBehaviour
{
    const int energy = 8;
    const float distance_absorb = 5f;
    const float speed = 20f;
    Vector2 drct,r_player;
    public GameObject player;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        this.enabled = false;
        Invoke(nameof(Wake), 1f);//����ʱ����1��
    }

    void Update()
    {
        r_player = player.transform.position-transform.position;
        drct=r_player.magnitude>distance_absorb?Vector2.zero: r_player.normalized;
        rb.velocity = drct * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.GetComponent<player>().CostEnergy(-energy);
            Destroy(this.gameObject);
        }
    }
    void Wake() => this.enabled = true;
}
