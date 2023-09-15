using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bomb : MonoBehaviour
{
    [Header("인풋")] // SetBomb에서 Instantiate시 받아옴
    public string playerTag; //설치자 태그
    public SetBomb setBomb; // 설치자 SetBomb
    public int blastLength = 1; // 폭발길이

    [Header("설정")]
    public float fuseTime = 3f; // 폭발시한
    public float timeDuration = 1f; // 지속시간
    public bool explode = true; //작동여부

    [Header("폭발")]
    public Explosion explosionPrefab;
    public LayerMask stageLayerMask; // 폭발 막힐 레이어마스크

    [Header("철거")]
    public Tilemap brickTiles;
    public Demolish demolishPrefab;

    //[Header("충돌무시")]
    //public bool placeFrame = true; // 폭탄설치한 프레임
    //public Collider2D playerCollider;
    //public Collider2D bombCollider;
    //public bool ignore = true;

    public void Start()
    {
        brickTiles = GameObject.Find("Brick").GetComponent<Tilemap>();
        //프리팹 내의 컴포넌트 변수는 인스턴스화되기 전에는 미리 참조할 수 없습니다.
        //이는 Unity의 동작 방식에 따른 것입니다.

        //bombCollider = GetComponent<Collider2D>();

        //Physics2D.IgnoreCollision(bombCollider, playerCollider, ignore);
        //이러면 플레이어끼리 겹쳤을때 폭탄설치하면 문제가 되네...

        //설치시 겹쳐진 캐릭터 받아와서 작동?이 되나?
        //StartCoroutine(PlaceFrame());
    }

    //OnCollisionEnter2D는 충돌처리 후 호출이라 첫충돌 무시가 안되서 실패
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("충돌");
        if(placeFrame && collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(bombCollider, collision.collider, true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Debug.Log("충돌끝");
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
        if (fuseTime > 0) // 타이머
        {
            fuseTime -= Time.deltaTime;
            // 즉시폭발하려면 코루틴 쓰면 안됨
        }
        else if (explode) // 타임오버시 한번만 작동
        {
            explode = false;

            Vector2 pos = transform.position;
            pos.x = Mathf.Round(pos.x);
            pos.y = Mathf.Round(pos.y);

            //중심 폭발
            Explosion explosion = Instantiate(explosionPrefab, pos, Quaternion.identity);
            explosion.SetActive(explosion.start);
            Destroy(explosion.gameObject, timeDuration);

            //각방향 폭발 재귀함수
            Explode(pos, Vector2.up, blastLength);
            Explode(pos, Vector2.down, blastLength);
            Explode(pos, Vector2.left, blastLength);
            Explode(pos, Vector2.right, blastLength);

            Destroy(gameObject); // 폭탄파괴
            if (setBomb != null)
            {
                setBomb.restBomb++; // 설치자의 잔여폭탄추가
            }
        }
    }
    private void Explode(Vector2 pos, Vector2 dir, int length)
    {
        if (length <= 0) return; // length가 0이되면 종료

        pos += dir; // 해당방향으로 +1칸에 작업

        if (Physics2D.OverlapBox(pos, Vector2.one / 2f, 0f, stageLayerMask)) // 벽이면
        {
            ClearBrick(pos); // 벽돌이면 철거

            return; // 폭발없이 종료
        }

        //줄기끝단 폭발
        Explosion explosion = Instantiate(explosionPrefab, pos, Quaternion.identity);
        explosion.SetActive(length > 1 ? explosion.middle : explosion.end);
        explosion.SetDir(dir);
        Destroy(explosion.gameObject, timeDuration);

        Explode(pos, dir, length - 1); // length 줄여서 재귀
    }

    private void ClearBrick(Vector2 pos)
    {
        // 해당 위치 벽돌타일 가져오기
        Vector3Int cell = brickTiles.WorldToCell(pos);
        TileBase tile = brickTiles.GetTile(cell);

        if (tile != null) // 벽돌이면 철거
        {
            Instantiate(demolishPrefab, pos, Quaternion.identity);
            brickTiles.SetTile(cell, null);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) // 폭발 충돌시 즉시 폭발
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Explosion"))
        {
            fuseTime = 0f;
        }
    }
}