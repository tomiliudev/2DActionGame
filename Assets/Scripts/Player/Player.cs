using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    [SerializeField] Rigidbody2D playerRg2d;
    [SerializeField] CapsuleCollider2D playerCollider;
    [SerializeField] float playerRunSpeed;
    [SerializeField] GroundCheck groundCheck;
    [SerializeField] AnimationCurve playerRunCurve;

    [SerializeField] float playerGravity;
    [SerializeField] float playerJumpSpeed;
    [SerializeField] float playerJumpLimitHight;
    [SerializeField] float playerJumpLimitTime;
    [SerializeField] GroundCheck headCheck;
    [SerializeField] AnimationCurve playerJumpCurve;
    [SerializeField] float stepOnRate;
    [SerializeField] float grippingPower;// 壁に掴む力


    [Header("Jump入力タイプ")][SerializeField] e_JumpInputType eJumpInputType = e_JumpInputType.upKeyDown;
    private enum e_JumpInputType
    {
        upKeyDown,
        upKeyKeep
    }

    GameManager gm;// GameManagerのインスタンス

    enum XPositionStatus
    {
        none,
        right,
        left
    };
    // xPositionStatusOverriteは進行方向を保持するため
    XPositionStatus xPositionStatusOverrite = XPositionStatus.none;
    XPositionStatus xPositionStatus = XPositionStatus.none;
    XPositionStatus beforeXPositionStatus = XPositionStatus.none;

    enum TouchType
    {
        runTouch,
        jumpTouch
    }

    private float _playerRunSpeed = 0f;
    private float dushTime;

    private bool isDie;
    private bool isJump;
    private bool canJumpHeight;
    private bool isGripWall;// 壁にへばり付く
    private bool iskickJump;// 壁にへばり付いている時にジャンプ
    private float kickJumpPos;// 壁キックした時点のｘポジション

    private enum e_WallDirection
    {
        none,
        right,
        left
    }
    private e_WallDirection wallDirection = e_WallDirection.none;// 触れた壁はプレイヤーの右なのか左なのか

    private float playerJumpPos;
    private float playerJumpTime;

    

    private float invincibleTime = 2f;
    private bool IsInvincible
    {
        get; set;
    }

    // 画面タッチのfingerIdを管理する
    Dictionary<TouchType, int> fingerIdDic = new Dictionary<TouchType, int>();


    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;

        // プレイヤー状態の初期化
        InitPlayerStatus();

        // fingerIdの初期化
        fingerIdDic.Add(TouchType.runTouch, -1);
        fingerIdDic.Add(TouchType.jumpTouch, -1);

        if (Application.isEditor)
        {
            playerRg2d.gravityScale = 0f;
        }
        else
        {
            playerRg2d.gravityScale = 5f;
        }
    }

    private void Update()
    {
        // スマホによる動きの操作（Run、Jump）
        DoMovementOperationByPhone();

        // キーボードによる動きの操作（Run、Jump）
        DoMovementOperationByKeyborad();

        if (gm.IsGameClear)
        {
            InitPlayerStatus();
        }
    }

    void FixedUpdate()
    {
        playerAnimator.SetBool("jump", isJump);
        playerAnimator.SetBool("ground", groundCheck.IsInGround);

        if (!gm.IsGameClear)
        {
            float velocity_x = Run();
            float velocity_y = Jump();

            // 壁にへばり付いてる時
            if (isGripWall)
            {
                velocity_y =　playerGravity - grippingPower;
                if (velocity_y < 0f)
                {
                    velocity_y = 0f;
                }
                else
                {
                    velocity_y = -velocity_y;
                }
            }

            if (iskickJump)
            {
                if (wallDirection == e_WallDirection.left && velocity_x < 0f) velocity_x = 0f;
                if (wallDirection == e_WallDirection.right && velocity_x > 0f) velocity_x = 0f;
            }

            if (!canJumpHeight || headCheck.IsInGround || groundCheck.IsInGround)
            {
                iskickJump = false;
            }

            playerRg2d.velocity = new Vector2(velocity_x, velocity_y);
        }
    }


    /// <summary>
    /// 横移動
    /// </summary>
    /// <returns></returns>
    float Run()
    {
        switch (xPositionStatus)
        {
            case XPositionStatus.right:
                playerAnimator.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
                playerAnimator.SetBool("run", true);
                _playerRunSpeed = playerRunSpeed;
                dushTime += Time.deltaTime;
                break;
            case XPositionStatus.left:
                playerAnimator.GetComponent<Transform>().localScale = new Vector3(-1f, 1f, 1f);
                playerAnimator.SetBool("run", true);
                _playerRunSpeed = -playerRunSpeed;
                dushTime += Time.deltaTime;
                break;
            default:
                playerAnimator.SetBool("run", false);
                _playerRunSpeed = 0f;
                dushTime = 0f;
                break;
        }

        if (beforeXPositionStatus != XPositionStatus.none && xPositionStatus != XPositionStatus.none && beforeXPositionStatus != xPositionStatus)
        {
            dushTime = 0f;
        }
        beforeXPositionStatus = xPositionStatus;

        _playerRunSpeed *= playerRunCurve.Evaluate(dushTime);

        return _playerRunSpeed;
    }


    /// <summary>
    /// ジャンプ
    /// </summary>
    /// <returns></returns>
    float Jump()
    {
        float _playerJumpSpeed = -playerGravity;

        if (isJump)
        {
            canJumpHeight = playerJumpPos + playerJumpLimitHight > transform.position.y;
            bool canTime = playerJumpLimitTime > playerJumpTime;

            if (canJumpHeight && canTime && !headCheck.IsInGround)
            {
                _playerJumpSpeed = playerJumpSpeed;
                playerJumpTime += Time.deltaTime;
            }
            else
            {
                isJump = false;
            }
        }

        if (isJump)
        {
            _playerJumpSpeed *= playerJumpCurve.Evaluate(playerJumpTime);
        }

        return _playerJumpSpeed;
    }


    /// <summary>
    /// 敵との接触判定
    /// プレイヤーの足元で踏みつけた場合は跳ねる
    /// それ以外の場合は敵に攻撃される
    /// </summary>
    /// <param name="collision"></param>
    void CheckContactJudgment(Collision2D collision)
    {
        float playerHeight = playerCollider.size.y;
        playerHeight = playerHeight * transform.localScale.y;// プレイヤーのスケールをかけてあげることで高さを求め

        // 踏みつける判定の高さ
        float stepOnHeight = playerHeight * (stepOnRate / 100f);
        float stepOnPos = transform.position.y - playerHeight / 2 + stepOnHeight;

        foreach (var contact in collision.contacts)
        {
            if (contact.point.y < stepOnPos)
            {
                ObjectCollision oc = collision.gameObject.GetComponent<ObjectCollision>();
                if (oc != null)
                {
                    oc.isPlayerStepOn = true;
                    playerRg2d.AddForce(transform.up * 500f);
                }
            }
            else
            {
                OnAttacked(collision);
            }
        }
    }


    /// <summary>
    /// 敵に触れた時の処理
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        CheckContactJudgment(collision);
    }

    /// <summary>
    /// 敵に触れ続ける時の処理
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        CheckContactJudgment(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GripWall(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GripWall(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            isGripWall = false;
            playerAnimator.SetBool("wallj", false);
            wallDirection = e_WallDirection.none;
        }
    }

    /// <summary>
    /// 壁をつかむ
    /// </summary>
    private void GripWall(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            if (collision.transform.position.x > transform.position.x)
            {
                wallDirection = e_WallDirection.right;
            }
            if (collision.transform.position.x < transform.position.x)
            {
                wallDirection = e_WallDirection.left;
            }

            /*
             * 壁にへばり付く条件
             * ジャンプして到達可能な高さになるまで
             * かつ地面に足がついていない時
             */
            if (!groundCheck.IsInGround && !isJump)
            {
                isGripWall = true;
                isJump = false;
                playerAnimator.SetBool("wallj", true);
            }
            else
            {
                isGripWall = false;
                playerAnimator.SetBool("wallj", false);
            }
        }
    }

    /// <summary>
    /// 敵に攻撃された時
    /// </summary>
    /// <param name="collision"></param>
    private void OnAttacked(Collision2D collision)
    {
        // 無敵期間中
        if (IsInvincible) return;

        // 死んだら何もしない
        if (isDie) return;

        // ゲームクリアしたら何もしない
        if (gm.IsGameClear) return;

        if (collision.gameObject.tag == "Enemy")
        {
            if (gm.PlayerCurrentHp > 0)
            {
                // 無敵時間
                StartCoroutine(DoInvincibleTime());

                // HPを１減らす
                gm.PlayerCurrentHp--;

                // PlayerHit
                playerAnimator.Play("PlayerHit");
            }

            if (gm.PlayerCurrentHp <= 0)
            {
                // 死んだ時
                isDie = true;
            }
        }
    }

    /// <summary>
    /// プレイヤーの初期状態の初期化
    /// </summary>
    private void InitPlayerStatus ()
    {
        isDie = false;
        isJump = false;

        playerRg2d.velocity = new Vector2(0f, 0f);
        playerAnimator.SetBool("run", false);
        playerAnimator.SetBool("jump", false);
        playerAnimator.SetBool("ground", true);
        playerAnimator.Play("PlayerIddle");
    }

    /// <summary>
    /// 攻撃された時の無敵時間
    /// </summary>
    /// <returns></returns>
    private IEnumerator DoInvincibleTime()
    {
        IsInvincible = true;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"));
        playerAnimator.SetBool("invincible", true);

        yield return new WaitForSeconds(invincibleTime);
        IsInvincible = false;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"), false);
        playerAnimator.SetBool("invincible", false);
    }

    /// <summary>
    /// タッチ情報を返却する
    /// </summary>
    /// <param name="touchType"></param>
    /// <returns></returns>
    private Touch GetTouchInfo(TouchType touchType)
    {
        Touch touch = Input.GetTouch(0);

        // 画面にタッチした指の数が２の場合
        if (Input.touchCount == 2)
        {
            if (fingerIdDic[touchType] == -1)
            {
                // fingerId==-1なら２本目のタッチ情報を取得する
                touch = Input.GetTouch(1);
            }
            else
            {
                // fingerIdがあるならInput.touchesから該当のfingerIdのタッチ情報を取得してくる
                touch = Input.touches.First(x => x.fingerId == fingerIdDic[touchType]);
            }
        }

        return touch;
    }

    /// <summary>
    /// スマホによる動きの操作（Run、Jump）
    /// </summary>
    private void DoMovementOperationByPhone()
    {
        if (Application.isEditor) return;
        DoRunByPhone();
        DoJumpByPhone();
    }

    /// <summary>
    /// スマホによる横移動
    /// </summary>
    private void DoRunByPhone()
    {
        xPositionStatus = XPositionStatus.none;
        if (Input.touchCount > 0)
        {
            TouchType touchType = TouchType.runTouch;
            Touch touch = GetTouchInfo(touchType);

            if (touch.position.x < Screen.width / 2)
            {
                // fingerIdを記録しておく
                fingerIdDic[touchType] = touch.fingerId;

                if (touch.deltaPosition.x > 1f)
                {
                    xPositionStatus = XPositionStatus.right;
                    xPositionStatusOverrite = XPositionStatus.right;
                }
                else if (touch.deltaPosition.x < -1f)
                {
                    xPositionStatus = XPositionStatus.left;
                    xPositionStatusOverrite = XPositionStatus.left;
                }
            }

            if (touch.phase == TouchPhase.Ended)
            {
                xPositionStatusOverrite = XPositionStatus.none;
            }

            /*
             * deltaPositionは前のPositionとの比較になるので
             * 指が動かなくなったらプレイヤーも止まってしまう
             * そのため、オーバーライドすることで一度方向が決まったら
             * 指が離れるまで、その方向で進むように制御できる
             */
            if (xPositionStatusOverrite != XPositionStatus.none)
            {
                xPositionStatus = xPositionStatusOverrite;
            }
        }
    }

    /// <summary>
    /// スマホによるジャンプ操作
    /// </summary>
    private void DoJumpByPhone()
    {
        if (Application.isEditor) return;
        if (Input.touchCount > 0)
        {
            if (groundCheck.IsInGround || isGripWall)
            {
                TouchType touchType = TouchType.jumpTouch;
                Touch touch = GetTouchInfo(touchType);

                if (touch.position.x > Screen.width / 2)
                {
                    // fingerIdを記録しておく
                    fingerIdDic[touchType] = touch.fingerId;
                    if (touch.phase == TouchPhase.Began)
                    {
                        DoJump();
                    }
                }
            }
        }
        else
        {
            // 画面に何もタッチしていない時にすべて-1でリセットする
            List<TouchType> keys = fingerIdDic.Keys.ToList();
            foreach (TouchType key in keys)
            {
                fingerIdDic[key] = -1;
            }
        }
    }

    /// <summary>
    /// キーボードによる動きの操作（Run、Jump）
    /// </summary>
    private void DoMovementOperationByKeyborad()
    {
        if (!Application.isEditor) return;
        DoRunByKeyborad();
        DoJumpByKeyborad();
    }

    /// <summary>
    /// キーボードによる移動操作
    /// </summary>
    private void DoRunByKeyborad()
    {
        float horizontalKey = Input.GetAxis("Horizontal");
        if (horizontalKey > 0f)
        {
            xPositionStatus = XPositionStatus.right;
        }
        else if (horizontalKey < 0f)
        {
            xPositionStatus = XPositionStatus.left;
        }
        else
        {
            xPositionStatus = XPositionStatus.none;
        }
    }

    /// <summary>
    /// キーボードによるジャンプ操作
    /// </summary>
    private void DoJumpByKeyborad()
    {
        switch (eJumpInputType)
        {
            case e_JumpInputType.upKeyDown:
                OnUpArrowDown();
                break;
            case e_JumpInputType.upKeyKeep:
                OnUpArrowKeep();
                break;
        }
    }

    /// <summary>
    /// 上キー押下によるジャンプ
    /// </summary>
    private void OnUpArrowDown()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (groundCheck.IsInGround || isGripWall)
            {
                DoJump();
            }
        }
    }

    /// <summary>
    /// 上キー継続押下によるジャンプ
    /// </summary>
    private void OnUpArrowKeep()
    {
        if (Input.GetAxis("Vertical") > 0f)
        {
            if (groundCheck.IsInGround || isGripWall)
            {
                DoJump();
            }
        }
        else
        {
            isJump = false;
        }
    }

    private void DoJump()
    {
        isJump = true;
        playerJumpPos = transform.position.y;
        playerJumpTime = 0f;

        if (isGripWall)
        {
            iskickJump = true;
            var kickJumpPos = transform.position;
            if (wallDirection == e_WallDirection.left)
            {
                transform.position = new Vector2(kickJumpPos.x + 0.5f, kickJumpPos.y);
            }
            if (wallDirection == e_WallDirection.right)
            {
                transform.position = new Vector2(kickJumpPos.x - 0.5f, kickJumpPos.y);
            }
        }
    }
}
