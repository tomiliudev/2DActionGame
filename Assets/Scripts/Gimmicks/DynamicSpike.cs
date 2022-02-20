using UnityEngine;

public sealed class DynamicSpike : DynamicSpikeBase
{
    enum Direction
    {
        up,
        down,
        left,
        right
    }
    [SerializeField] Direction direction;
    [SerializeField] float distance = 2f;
    [SerializeField] bool isTorigger;

    GameManager gm;
    RaycastHit2D hit;
    int PlayerLayerMask;

    Vector2 originPos;
    float originYpos;
    float originXpos;

    private void Start()
    {
        gm = GameManager.Instance;

        PlayerLayerMask = 1 << LayerMask.NameToLayer("Player");

        originPos = transform.position;
        originXpos = transform.position.x;
        originYpos = transform.position.y;

        if (!isTorigger)
        {
            // 自動発動
            UpAnime();
        }
    }

    private void Update()
    {
        CheckTorigger();
    }

    private void CheckTorigger()
    {
        if (!isTorigger) return;

        Vector2 dir = (Vector2)gm.player.transform.position - originPos;
        hit = Physics2D.Raycast(originPos, dir, distance, PlayerLayerMask);
        //Debug.DrawRay(originPos, dir * distance, Color.red);

        if (hit.collider != null && LayerMask.LayerToName(hit.collider.gameObject.layer) == "Player")
        {
            if (!isAniming) UpAnime();
        }
    }

    // アニメーション中
    bool isAniming;
    private void UpAnime()
    {
        isAniming = true;

        SoundManager.Instance.Play(se);

        float moveToX = originXpos + 1;
        float moveToY = originYpos + 1f;
        switch (direction)
        {
            case Direction.up:
                moveToX = originXpos + 0f;
                moveToY = originYpos + 1f;
                break;
            case Direction.down:
                moveToX = originXpos + 0f;
                moveToY = originYpos - 1f;
                break;
            case Direction.left:
                moveToX = originXpos - 1;
                moveToY = originYpos + 0f;
                break;
            case Direction.right:
                moveToX = originXpos + 1;
                moveToY = originYpos + 0f;
                break;
        }

        iTween.MoveTo(gameObject,
            iTween.Hash(
                "x", moveToX,
                "y", moveToY,
                "time", 1f,
                "easeType", iTween.EaseType.easeOutElastic,
                "oncomplete", "OnUpAnimeComplete"
            )
        );
    }

    private void DownAnime()
    {
        iTween.MoveTo(gameObject,
            iTween.Hash(
                "x", originXpos,
                "y", originYpos,
                "time", 2f,
                "easeType", iTween.EaseType.easeOutSine,
                "oncomplete", "OnDownAnimeComplete"
            )
        );
    }

    private void OnUpAnimeComplete()
    {
        DownAnime();
    }

    private void OnDownAnimeComplete()
    {
        isAniming = false;

        if (!isTorigger)
        {
            Invoke("UpAnime", 2f);
        }
    }
}
