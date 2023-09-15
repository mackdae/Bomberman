using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{
    [Header("��ǲ")] // SetBomb���� Instantiate�� �޾ƿ�
    public string playerTag; //��ġ�� �±�
    public SetBomb setBomb; // ��ġ�� SetBomb
    public int blastLength = 1; // ���߱���

    [Header("����")]
    public float fuseTime = 3f; // ���߽���
    public float timeDuration = 1f; // ���ӽð�
    public bool explode = true; //�۵�����

    [Header("����")]
    public Explosion explosionPrefab;
    public LayerMask stageLayerMask; // ���� ���� ���̾��ũ

    [Header("ö��")]
    public Tilemap brickTiles;
    public Demolish demolishPrefab;

    //[Header("�浹����")]
    //public bool placeFrame = true; // ��ź��ġ�� ������
    //public Collider2D playerCollider;
    //public Collider2D bombCollider;
    //public bool ignore = true;

    public void Start()
    {
        brickTiles = GameObject.Find("Brick").GetComponent<Tilemap>();
        //������ ���� ������Ʈ ������ �ν��Ͻ�ȭ�Ǳ� ������ �̸� ������ �� �����ϴ�.
        //�̴� Unity�� ���� ��Ŀ� ���� ���Դϴ�.

        //bombCollider = GetComponent<Collider2D>();

        //Physics2D.IgnoreCollision(bombCollider, playerCollider, ignore);
        //�̷��� �÷��̾�� �������� ��ź��ġ�ϸ� ������ �ǳ�...

        //��ġ�� ������ ĳ���� �޾ƿͼ� �۵�?�� �ǳ�?
        //StartCoroutine(PlaceFrame());
    }

    //OnCollisionEnter2D�� �浹ó�� �� ȣ���̶� ù�浹 ���ð� �ȵǼ� ����
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("�浹");
        if(placeFrame && collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(bombCollider, collision.collider, true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("�浹��");
        Physics2D.IgnoreCollision(bombCollider, collision.collider, false);
    }

    private IEnumerator PlaceFrame()
    {
        yield return null;
        yield return null;
        placeFrame = false;
    }
    */

    public void Update()
    {
        if (fuseTime > 0) // Ÿ�̸�
        {
            fuseTime -= Time.deltaTime;
            // ��������Ϸ��� �ڷ�ƾ ���� �ȵ�
        }
        else if (explode) // Ÿ�ӿ����� �ѹ��� �۵�
        {
            explode = false;

            Vector2 pos = transform.position;
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);

            //�߽� ����
            Explosion explosion = Instantiate(explosionPrefab, pos, Quaternion.identity);
            explosion.SetActive(explosion.start);
            Destroy(explosion.gameObject, timeDuration);

            //������ ���� ����Լ�
            Explode(pos, Vector2.up, blastLength);
            Explode(pos, Vector2.down, blastLength);
            Explode(pos, Vector2.left, blastLength);
            Explode(pos, Vector2.right, blastLength);

            Destroy(gameObject); // ��ź�ı�
            if (setBomb != null)
            {
                setBomb.restBomb++; // ��ġ���� �ܿ���ź�߰�
            }
        }
    }
    private void Explode(Vector2 pos, Vector2 dir, int length)
    {
        if (length <= 0) return; // length�� 0�̵Ǹ� ����

        pos += dir; // �ش�������� +1ĭ�� �۾�

        if (Physics2D.OverlapBox(pos, Vector2.one / 2f, 0f, stageLayerMask)) // ���̸�
        {
            ClearBrick(pos); // �����̸� ö��

            return; // ���߾��� ����
        }

        //�ٱⳡ�� ����
        Explosion explosion = Instantiate(explosionPrefab, pos, Quaternion.identity);
        explosion.SetActive(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDir(dir);
        Destroy(explosion.gameObject, timeDuration);

        Explode(pos, dir, length - 1); // length �ٿ��� ���
    }

    private void ClearBrick(Vector2 pos)
    {
        // �ش� ��ġ ����Ÿ�� ��������
        Vector3Int cell = brickTiles.WorldToCell(pos);
        TileBase tile = brickTiles.GetTile(cell);

        if (tile != null) // �����̸� ö��
        {
            Instantiate(demolishPrefab, pos, Quaternion.identity);
            brickTiles.SetTile(cell, null);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // ���� �浹�� ��� ����
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            fuseTime = 0f;
        }
    }
}