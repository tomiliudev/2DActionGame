using System.Linq;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] e_MoveType moveType;
    [SerializeField] float moveSpeed = 3f;

    [SerializeField] float moveDistance = 3f;
    [SerializeField] e_FirstDirectionType firstDirection;

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

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        cameraCollider = gm.cameraCollider;

        position = transform.position;
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

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
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

        rb2d.MovePosition(Vector2.MoveTowards(transform.position, movePos, moveSpeed * Time.fixedDeltaTime));

        // 速度 = 距離 / 時間
        SelfVelocity = (rb2d.position - oldPos) / Time.fixedDeltaTime;
        oldPos = rb2d.position;
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
