using UnityEngine;

public sealed class DynamicSpike : MonoBehaviour
{
    enum Direction
    {
        up,
        down,
        left,
        right
    }
    [SerializeField] Direction direction;

    float originYpos;
    float originXpos;
    private void Start()
    {
        originXpos = transform.position.x;
        originYpos = transform.position.y;
        
        UpAnime();
    }

    private void UpAnime()
    {
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
                moveToX = originXpos + 1;
                moveToY = originYpos + 0f;
                break;
            case Direction.right:
                moveToX = originXpos - 1;
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
        Invoke("UpAnime", 2f);
    }
}
