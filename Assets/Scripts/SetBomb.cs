using UnityEngine;

public class SetBomb : MonoBehaviour
{
    [Header("����")]
    public KeyCode inputBomb = KeyCode.Space;
    public GameObject bombPrefab;
    public int blastLength = 1; // ���߱���
    public int maxBomb = 1; // �ִ���ź��
    public int restBomb; // �ܿ���ź��

    void OnEnable()
    {
        restBomb = maxBomb;
    }

    void Update()
    {
        if (restBomb > 0 && Input.GetKeyDown(inputBomb)) // Ű�Է½�
        {
            PlaceBomb(); // ��ź��ġ
        }
    }

    private void PlaceBomb()
    {
        Vector2 pos = transform.position;
        pos.x = Mathf.Round(pos.x);
        pos.y = Mathf.Round(pos.y);

        restBomb--;
        Bomb bomb = Instantiate(bombPrefab, pos, Quaternion.identity).GetComponent<Bomb>();
        
        if (bomb != null)
        {
            bomb.blastLength = blastLength;
            bomb.setBomb = this;
            bomb.playerTag = gameObject.tag;
            //bomb.playerCollider = GetComponent<Collider2D>();
        }
    }
    public void AddBomb()
    {
        maxBomb++;
        restBomb++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //��ź ������ ����� Ʈ���� ��
        if(collision.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            collision.isTrigger = false;
        }
        //�̷��� �÷��̾ �ö� ���¿��� �ٸ��÷��̾ �и� ��ź ���� �ǳʴٴ�
    }
    // ���� ��ź���� IgnoreCollision �۵��ϰ� �����ϰ� �;��µ� ����
}