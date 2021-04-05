using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public CapsuleCollider2D coll;
    private Animator anim;
    public LayerMask ground;//地面信息
    public Text cherryNumber;
    public AudioSource jumpAudio,dieAudio;
    public GameObject Messagebox;
    public AudioMixer mainMixer;

    public float speed, jumpSpeed;//初速度，起跳初速度
    int jumpCount;//还可以跳跃的次数
    bool isGround,isTouchingCherry;
    float horizontalDirectiion;//水平移动方向
    int count,cherry=0;
    public bool jumpPressed;//是否按下跳跃键
    Menu menu;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        mainMixer.SetFloat("MainVolume", -20);
        menu = GameObject.Find("Menu").GetComponent<Menu>();
    }
    void Update()
    {
        if (Input.GetButtonDown("Jump")) jumpPressed = true;
    }
    void FixedUpdate()
    {
        isGround = coll.IsTouchingLayers(ground);
        GroundMove();
        Jump();
        SwitchAnim();
    }
    void GroundMove()
    {
        horizontalDirectiion = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalDirectiion * speed, rb.velocity.y);
        anim.SetBool("run", horizontalDirectiion != 0);
        if (anim.GetBool("run"))
        {
            transform.localScale = new Vector3(horizontalDirectiion, 1, 1);
        }
    }
    void Jump()
    {
        if (isGround)
        {
            jumpCount = 2;
        }
        if (jumpCount>0&&jumpPressed)
        {
            jumpAudio.Play();
            jumpCount--;
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            rb.position = new Vector3(rb.position.x, rb.position.y+0.01f, -4);
        }
        jumpPressed = false;//在确认能否跳起而不是经过下一帧时重置jumpPressed能改善手感
    }
    void SwitchAnim()
    {
        if (isGround)
        {
            anim.SetBool("jump", false);
            anim.SetBool("fall", false);
            anim.SetBool("idle", true);
        }   
        else
        {
            anim.SetBool("fall", rb.velocity.y < 0);
            anim.SetBool("jump", rb.velocity.y > 0);
            anim.SetBool("idle", false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collection"))
        {
            if (collision.GetType() == typeof(BoxCollider2D))
            {
                Destroy(collision.gameObject);
                cherry++;
                cherryNumber.text = cherry.ToString();
            }
        }
        else if (collision.CompareTag("Edge"))
        {
            Messagebox.SetActive(true);
            mainMixer.SetFloat("MainVolume", -80);
            dieAudio.Play();
            menu.Restart();
        }
    }
}
