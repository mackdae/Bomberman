using UnityEngine;

public class SetBomb : MonoBehaviour
{
    [Header("설정")]
    public KeyCode inputBomb = KeyCode.Space;
    public GameObject bombPrefab;
    public int blastLength = 1; // 폭발길이
    public int maxBomb = 1; // 최대폭탄수
    public int restBomb; // 잔여폭탄수

    void OnEnable()
    {
        restBomb = maxBomb;
    }

    void Update()
    {
        if (restBomb > 0 && Input.GetKeyDown(inputBomb)) // 키입력시
        {
            PlaceBomb(); // 폭탄설치
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
        //폭탄 위에서 벗어나면 트리거 끔
        if(collision.gameObject.layer == LayerMask.NameToLayer("Bomb"))
        {
            collision.isTrigger = false;
        }
        //이러면 플레이어가 올라간 상태에서 다른플레이어가 밀면 폭탄 위로 건너다님
    }
    // 개별 폭탄에서 IgnoreCollision 작동하게 수정하고 싶었는데 실패
}