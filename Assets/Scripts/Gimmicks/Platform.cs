using System.Linq;
using UnityEngine;

public sealed class Platform : MonoBehaviour
{
    [SerializeField] Transform movString;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] PlatformEffector2D pe2d;

    [SerializeField] e_MoveType moveType;
    [SerializeField] float moveSpeed = 3f;

    [SerializeField] float moveDistance = 3f;
    [SerializeField] e_FirstDirectionType firstDirection;

    [Header("消える足場")]
    [SerializeField] bool isHidePlatform;
    [SerializeField] float hideSpeed;// 消えるスピード
    [SerializeField] float hideInterval;// 非表示の時間間隔
    [SerializeField] float appearInterval;// 表示の時間間隔
    [SerializeField] e_HideType firstHideType;
    [SerializeField] float delayTime;// 遅延実行時間

    GameManager gm;
    PolygonCollider2D cameraCollider;
    Vector2 movePos;// 移動目標Position

    private enum e_MoveType
    {
        freeze,
        up,
        down,
        left,
        right,
        vertical,// 上下移動
        horizon// 左右移動
    }
    Vector2 direction = Vector2.up;// 移動方向

    /// <summary>
    /// e_MoveTypeがverticalまたはhorizonの時に、最初に移動する方向
    /// </summary>
    private enum e_FirstDirectionType
    {
        up,
        down,
        left,
        right
    }

    private Vector2 position;
    
    private float cameraColliderUpY;
    private float cameraColliderDownY;

    private float targetPositionX;// 目標X座標
    private float targetPositionY;// 目標Y座標

    private Vector2 oldPos = Vector2.zero;
    public Vector2 SelfVelocity
    {
        get; private set;
    }

    private Color selfColor;
    float alpha = 0f;
    private enum e_HideType
    {
        hide,
        appear
    }
    private e_HideType hideType = e_HideType.hide;
    private float intervalTime;
    private float _delayTime;
    private bool isSetLayerMask = false;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;

        if (movString == null)
        {
            cameraCollider = gm.cameraCollider;

            position = transform.position;
            selfColor = sr.color;

            hideType = firstHideType;

            SetupFirstMovePos();
            SetupFirstHideMode();
        }
        else
        {
            SetupToriggerMoveFirstDirction();
        }
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (movString == null)
        {
            SetMoveParams();
            UpdateHideMode();
        }
        else
        {

        }

        Move();
    }

    private void SetupFirstMovePos()
    {
        switch (moveType)
        {
            case e_MoveType.freeze:
                return;
            case e_MoveType.up:
                movePos = new Vector2(position.x, cameraCollider.points.ElementAt(0).y + 1f);
                break;
            case e_MoveType.down:
                movePos = new Vector2(position.x, cameraCollider.points.ElementAt(2).y - 1f);
                break;
            case e_MoveType.left:
                break;
            case e_MoveType.right:
                break;
            case e_MoveType.vertical:
                switch (firstDirection)
                {
                    case e_FirstDirectionType.up:
                        direction = Vector2.up;
                        targetPositionY = position.y + moveDistance;
                        break;
                    case e_FirstDirectionType.down:
                        direction = Vector2.down;
                        targetPositionY = position.y - moveDistance;
                        break;
                }
                movePos = new Vector2(position.x, targetPositionY);
                break;
            case e_MoveType.horizon:
                switch (firstDirection)
                {
                    case e_FirstDirectionType.left:
                        direction = Vector2.left;
                        targetPositionX = position.x - moveDistance;
                        break;
                    case e_FirstDirectionType.right:
                        direction = Vector2.right;
                        targetPositionX = position.x + moveDistance;
                        break;
                }
                movePos = new Vector2(targetPositionX, position.y);
                break;
        }
    }

    private void SetupToriggerMoveFirstDirction()
    {
        switch (firstDirection)
        {
            case e_FirstDirectionType.up:
                direction = Vector2.up;
                break;
            case e_FirstDirectionType.down:
                direction = Vector2.down;
                break;
            case e_FirstDirectionType.left:
                direction = Vector2.left;
                break;
            case e_FirstDirectionType.right:
                direction = Vector2.right;
                break;
        }

        movePos = transform.position;
        targetPositionY = movePos.y;
    }

    private void SetMoveParams()
    {
        switch (moveType)
        {
            case e_MoveType.freeze:
                return;
            case e_MoveType.up:
                break;
            case e_MoveType.down:
                break;
            case e_MoveType.left:
                break;
            case e_MoveType.right:
                break;
            case e_MoveType.vertical:
                if (direction == Vector2.up && transform.position.y >= targetPositionY)
                {
                    direction = Vector2.down;
                    targetPositionY = targetPositionY - moveDistance;
                    movePos = new Vector2(transform.position.x, targetPositionY);
                }
                if (direction == Vector2.down && transform.position.y <= targetPositionY)
                {
                    direction = Vector2.up;
                    targetPositionY = targetPositionY + moveDistance;
                    movePos = new Vector2(transform.position.x, targetPositionY);
                }
                break;
            case e_MoveType.horizon:
                if (direction == Vector2.left && transform.position.x <= targetPositionX)
                {
                    direction = Vector2.right;
                    targetPositionX = targetPositionX + moveDistance;
                    movePos = new Vector2(targetPositionX, transform.position.y);
                }
                if (direction == Vector2.right && transform.position.x >= targetPositionX)
                {
                    direction = Vector2.left;
                    targetPositionX = targetPositionX - moveDistance;
                    movePos = new Vector2(targetPositionX, transform.position.y);
                }
                break;
        }

        /*
         * Rigidbodyは物理演算を行うため、transformを使っての移動は物理演算を再計算してしまうので重くなる原因で避けるべき。
         */
        //transform.Translate(direction * moveSpeed * Time.fixedDeltaTime);
    }

    private void SetToriggerMoveParams()
    {
        if (movString == null) return;
        if (movePos.y != targetPositionY) return;

        if (direction == Vector2.up)
        {
            direction = Vector2.down;
            targetPositionY = movString.GetComponent<MovString>().Top;
        }
        else if (direction == Vector2.down)
        {
            direction = Vector2.up;
            targetPositionY = movString.GetComponent<MovString>().Bottom;
        }

        movePos = new Vector2(transform.position.x, targetPositionY);
    }

    private void Move()
    {
        if (moveType == e_MoveType.freeze) return;
        rb2d.MovePosition(Vector2.MoveTowards(transform.position, movePos, moveSpeed * Time.fixedDeltaTime));

        // 速度 = 距離 / 時間
        SelfVelocity = (rb2d.position - oldPos) / Time.fixedDeltaTime;
        oldPos = rb2d.position;
    }

    private void SetupFirstHideMode()
    {
        if (!isHidePlatform) return;
        switch (firstHideType)
        {
            case e_HideType.hide:
                sr.color = new Color(selfColor.r, selfColor.g, selfColor.b, 0f);
                pe2d.colliderMask = new LayerMask();
                break;
            case e_HideType.appear:
                sr.color = new Color(selfColor.r, selfColor.g, selfColor.b, 1f);
                pe2d.colliderMask = ~0;//(Everything)
                break;
        }
    }

    /// <summary>
    /// 表示モードを更新
    /// </summary>
    private void UpdateHideMode()
    {
        if (isHidePlatform)
        {
            _delayTime += Time.fixedDeltaTime;
            if (_delayTime < delayTime) return;
            switch (hideType)
            {
                case e_HideType.hide:
                    if (sr.color.a <= 0f)
                    {
                        intervalTime += Time.fixedDeltaTime;
                        if (intervalTime >= hideInterval)
                        {
                            hideType = e_HideType.appear;
                            intervalTime = 0f;
                        }
                    }
                    else
                    {
                        alpha += -hideSpeed * Time.fixedDeltaTime;
                    }
                    break;
                case e_HideType.appear:
                    if (sr.color.a >= 1f)
                    {
                        intervalTime += Time.fixedDeltaTime;
                        if (intervalTime >= appearInterval)
                        {
                            hideType = e_HideType.hide;
                            intervalTime = 0f;
                        }
                    }
                    else
                    {
                        alpha += hideSpeed * Time.fixedDeltaTime;
                    }
                    break;
            }
            sr.color = new Color(selfColor.r, selfColor.g, selfColor.b, alpha);
            if (!isSetLayerMask && sr.color.a > 0f)
            {
                isSetLayerMask = true;
                pe2d.colliderMask = ~0;//(Everything)
            }
            if(isSetLayerMask && sr.color.a <= 0f)
            {
                isSetLayerMask = false;
                pe2d.colliderMask = new LayerMask();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == GameConfig.PlayerFootTag)
        {
            Debug.Log("aaaaaaabbbbbbbbbccccccccc");
            SetToriggerMoveParams();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "CameraCollider")
        {
            var cameraCollider = collision.GetComponent<PolygonCollider2D>();
            cameraColliderUpY = cameraCollider.points.ElementAt(1).y;
            cameraColliderDownY = cameraCollider.points.ElementAt(2).y;
            switch (moveType)
            {
                case e_MoveType.up:
                    transform.position = new Vector2(position.x, cameraColliderDownY);
                    break;
                case e_MoveType.down:
                    transform.position = new Vector2(position.x, cameraColliderUpY);
                    break;
                case e_MoveType.left:
                    break;
                case e_MoveType.right:
                    break;
                case e_MoveType.vertical:
                    break;
                case e_MoveType.horizon:
                    break;
            }
        }
    }
}
