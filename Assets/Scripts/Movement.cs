using System.Collections;
using UnityEngine;


public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    private Vector2 dir = Vector2.down;
    public float speed = 5f;
    public bool kick = false;

    public KeyCode inputUp = KeyCode.W;
    public KeyCode inputDown = KeyCode.S;
    public KeyCode inputLeft = KeyCode.A;
    public KeyCode inputRight = KeyCode.D;

    public Animated animatedUp;
    public Animated animatedDown;
    public Animated animatedLeft;
    public Animated animatedRight;
    public Animated animatedDie;
    private Animated animatedIdle;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animatedIdle = animatedDown;
    }

    void Update()
    {
        if (Input.GetKey(inputUp))
        {
            SetDir(Vector2.up, animatedUp);
        }
        else if (Input.GetKey(inputDown))
        {
            SetDir(Vector2.down, animatedDown);
        }
        else if (Input.GetKey(inputLeft))
        {
            SetDir(Vector2.left, animatedLeft);
        }
        else if (Input.GetKey(inputRight))
        {
            SetDir(Vector2.right, animatedRight);
        }
        else
        {
            SetDir(Vector2.zero, animatedIdle);
        }
    }

    private void SetDir(Vector2 newDir, Animated animated) // 키입력 받아서 방향셋
    {
        dir = newDir;

        //애니메이션 활성비활성
        animatedUp.enabled = animated == animatedUp;
        animatedDown.enabled = animated == animatedDown;
        animatedLeft.enabled = animated == animatedLeft;
        animatedRight.enabled = animated == animatedRight;

        animatedIdle = animated;
        animatedIdle.idle = dir == Vector2.zero;
    }

    private void FixedUpdate()
    {
        //이동
        Vector2 pos = rb.position;
        Vector2 trans = dir * speed * Time.fixedDeltaTime;
        rb.MovePosition(pos + trans);
    }

    private void OnTriggerEnter2D(Collider2D collision) // 폭발 충돌시 사망
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die() //사망처리 코루틴
    {
        //기능 비활성
        enabled = false;
        GetComponent<SetBomb>().enabled = false;

        //애니 활성비활성
        animatedUp.enabled = false;
        animatedDown.enabled = false;
        animatedLeft.enabled = false;
        animatedRight.enabled = false;
        animatedDie.enabled = true;

        yield return new WaitForSeconds(1.25f); //애니재생후

        gameObject.SetActive(false); //플레이어비활성
        FindObjectOfType<GameManager>().CheckAlive(); // 게임메니저 생존체크 호출
    }

    // 반복호출이 아니면 인보크로 할 필요가 있나?
    /*
    private void Die() //사망처리
    {
        enabled = false;
        GetComponent<Bomb>().enabled = false;

        animatedUp.enabled = false;
        animatedDown.enabled = false;
        animatedLeft.enabled = false;
        animatedRight.enabled = false;
        animatedDie.enabled = true;

        Invoke(nameof(AfterDie), 1.25f); // 1.25초 뒤 사망후처리
    }
    private void AfterDie() //사망후처리
    {
        gameObject.SetActive(false);
        FindObjectOfType<GameManager>().CheckWinState();
    }
    */

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            if (kick)
                collision.rigidbody.bodyType = RigidbodyType2D.Dynamic;
            else
                collision.rigidbody.bodyType = RigidbodyType2D.Static;
        }
    }
}