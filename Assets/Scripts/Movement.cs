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

    private void SetDir(Vector2 newDir, Animated animated) // Ű�Է� �޾Ƽ� �����
    {
        dir = newDir;

        //�ִϸ��̼� Ȱ����Ȱ��
        animatedUp.enabled = animated == animatedUp;
        animatedDown.enabled = animated == animatedDown;
        animatedLeft.enabled = animated == animatedLeft;
        animatedRight.enabled = animated == animatedRight;

        animatedIdle = animated;
        animatedIdle.idle = dir == Vector2.zero;
    }

    private void FixedUpdate()
    {
        //�̵�
        Vector2 pos = rb.position;
        Vector2 trans = dir * speed * Time.fixedDeltaTime;
        rb.MovePosition(pos + trans);
    }

    private void OnTriggerEnter2D(Collider2D collision) // ���� �浹�� ���
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die() //���ó�� �ڷ�ƾ
    {
        //��� ��Ȱ��
        enabled = false;
        GetComponent<SetBomb>().enabled = false;

        //�ִ� Ȱ����Ȱ��
        animatedUp.enabled = false;
        animatedDown.enabled = false;
        animatedLeft.enabled = false;
        animatedRight.enabled = false;
        animatedDie.enabled = true;

        yield return new WaitForSeconds(1.25f); //�ִ������

        gameObject.SetActive(false); //�÷��̾��Ȱ��
        FindObjectOfType<GameManager>().CheckAlive(); // ���Ӹ޴��� ����üũ ȣ��
    }

    // �ݺ�ȣ���� �ƴϸ� �κ�ũ�� �� �ʿ䰡 �ֳ�?
    /*
    private void Die() //���ó��
    {
        enabled = false;
        GetComponent<Bomb>().enabled = false;

        animatedUp.enabled = false;
        animatedDown.enabled = false;
        animatedLeft.enabled = false;
        animatedRight.enabled = false;
        animatedDie.enabled = true;

        Invoke(nameof(AfterDie), 1.25f); // 1.25�� �� �����ó��
    }
    private void AfterDie() //�����ó��
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