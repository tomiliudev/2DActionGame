using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class Player : MonoBehaviour
    , ILeftButton
    , IRightButton
    , IUpButton
    , IDownButton
{
    [SerializeField] AudioClip jumpSe;
    [SerializeField] AudioClip bowSe;

    [SerializeField] Animator playerAnimator;
    [SerializeField] Rigidbody2D playerRg2d;
    [SerializeField] CapsuleCollider2D playerCollider;
    [SerializeField] float playerRunSpeed;
    [SerializeField] GroundCheck groundCheck;
    [SerializeField] AnimationCurve playerRunCurve;

    [SerializeField] float playerGravity;
    [SerializeField] float playerJumpSpeed;
    [SerializeField] float playerMushroomJumpSpeed;
    [SerializeField] float playerJumpLimitHight;
    [SerializeField] float playerJumpLimitTime;
    [SerializeField] float playerClimbSpeed;
    [SerializeField] GroundCheck headCheck;
    [SerializeField] AnimationCurve playerJumpCurve;
    [SerializeField] float stepOnRate;
    [SerializeField] float grippingPower;// 壁に掴む力

    [SerializeField] SpriteRenderer bowLine;
    [SerializeField] SpriteRenderer bow;

    [SerializeField] ParticleSystem dust;
    [SerializeField] Vector2 DamageCausedKnockbackForce = new Vector2(10f, 0f);

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
    
    XPositionStatus xPositionStatus = XPositionStatus.none;
    XPositionStatus beforeXPositionStatus = XPositionStatus.none;

    // 右に向いてるか
    public bool IsOnRight
    {
        get
        {
            var trans = playerAnimator.GetComponent<Transform>();
            return trans.localScale.x > 0f;
        }
    }

    enum TouchType
    {
        runTouch,
        jumpTouch
    }

    private Vector2 playerVelocity;
    private float _playerRunSpeed = 0f;
    private float dushTime;

    private bool isDie;
    private bool isJump;
    private bool canJumpHeight;
    private bool isGripWall;// 壁にへばり付く
    private bool iskickJump;// 壁にへばり付いている時にジャンプ

    private enum e_WallDirection
    {
        none,
        right,
        left
    }
    private e_WallDirection wallDirection = e_WallDirection.none;// 触れた壁はプレイヤーの右なのか左なのか

    bool isCliming;
    bool canClimbUp;
    bool canClimbDown;
    float ladderTopPos;
    float ladderBottomPos;
    Vector2 ladderCenter = Vector2.zero;
    Vector2 ladderExtents = Vector2.zero;
    float playerBottomPos;
    enum e_ClimbType
    {
        none,
        climbUp,
        climbDown
    }
    e_ClimbType climbType = e_ClimbType.none;

    private float playerJumpPos;
    private float playerJumpTime;
    private float jumpLimitHight;

    private float invincibleTime = 0.5f;
    private bool IsInvincible
    {
        get; set;
    }

    Vector2 knockBackForce = Vector2.zero;

    // 画面タッチのfingerIdを管理する
    Dictionary<TouchType, int> fingerIdDic = new Dictionary<TouchType, int>();

    // 触れている宝箱
    public Treasure TouchingTreasure { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;

        // プレイヤー状態の初期化
        InitPlayerStatus();

        // fingerIdの初期化
        fingerIdDic.Add(TouchType.runTouch, -1);
        fingerIdDic.Add(TouchType.jumpTouch, -1);
    }

    private void Update()
    {
        if (GameUtility.Instance.IsGamePause) return;

        if (!isDie)
        {
            // スマホによる動きの操作（Run、Jump）
            DoMovementOperationByPhone();

            // キーボードによる動きの操作（Run、Jump）
            DoMovementOperationByKeyborad();

            FallDownOneWayPlatform();
        }

        if (gm.IsGameClear)
        {
            InitPlayerStatus();
        }
    }

    void FixedUpdate()
    {
        // カメラの移動範囲を超えるとジャンプ無効に
        if (gm.cameraCollider != null && headCheck.transform.position.y > gm.cameraCollider.points[0].y)
        {
            isJump = false;
        }

        playerAnimator.SetBool("jump", isJump);
        playerAnimator.SetBool("ground", groundCheck.IsInGround);

        if (!gm.IsGameClear)
        {
            // プレイヤー向きの更新
            UpdatePlayerDirection();

            //// プレイヤーの動きの処理
            UpdateMovement();

            //// プレイヤーの無敵状況を更新
            UpdateInvincibleInfo();
        }
    }

    float rayMaxDistance = 100f;
    Vector2 diffPos;
    Vector2 direction;
    float bowLineFadeTime = 0.1f;
    public void BowAttack()
    {
        // 壁にへばり付いている時は矢を放てない
        if (isGripWall) return;

        SoundManager.Instance.Play(bowSe);

        var trans = playerAnimator.GetComponent<Transform>();
        direction = trans.localScale.x > 0f ? Vector2.right : Vector2.left;

        Vector2 bowStartPosition = transform.position - new Vector3(0f, 0.2f, 0f);

        var hit = Physics2D.Raycast(bowStartPosition, direction, rayMaxDistance,
            1 << LayerMask.NameToLayer("Enemy")
            | 1 << LayerMask.NameToLayer("Ground")
            | 1 << LayerMask.NameToLayer("Box")
        );
        
        if (hit.collider != null)
        {
            diffPos = hit.point - (Vector2)transform.position;
            ShowBowLine(Mathf.Abs(diffPos.x), bowStartPosition, hit.point);

            if (hit.transform.tag == GameConfig.GroundTag)
            {
                SpriteRenderer _bow = Instantiate(bow);
                float _bowWidth = _bow.size.x;
                Vector2 _bowPosition = new Vector2(
                    direction == Vector2.right ? hit.point.x - _bowWidth / 2 : hit.point.x + _bowWidth / 2
                    , bowStartPosition.y
                );

                _bow.transform.position = _bowPosition;
                _bow.transform.localScale = new Vector3(trans.localScale.x, _bow.transform.localScale.y);
            }
            else if(hit.transform.tag == GameConfig.EnemyTag)
            {
                hit.transform.GetComponent<Enemy>().OnDamage();
            }
            else if (hit.transform.tag == GameConfig.BoxTag)
            {
                hit.transform.GetComponent<Box>().OnDamage();
            }

            //Debug.DrawRay(bowStartPosition, direction * Mathf.Abs(diffPos.x), Color.red);
        }
        else
        {
            Vector2 hitPos;
            if(direction == Vector2.right)
            {
                hitPos = (Vector2)transform.position + new Vector2(rayMaxDistance, 0);
            }
            else
            {
                hitPos = (Vector2)transform.position - new Vector2(rayMaxDistance, 0);
            }
            ShowBowLine(rayMaxDistance, bowStartPosition, hitPos);
        }

        playerAnimator.SetTrigger("bow");
    }

    private void ShowBowLine(float sizeX, Vector2 bowStartPosition, Vector2 hitPos)
    {
        SpriteRenderer _bowLine = Instantiate(bowLine);
        _bowLine.size = new Vector2(sizeX, _bowLine.size.y);
        _bowLine.transform.position = (hitPos + bowStartPosition) / 2;
        iTween.FadeTo(_bowLine.gameObject, 0f, bowLineFadeTime);
        Destroy(_bowLine.gameObject, bowLineFadeTime);
    }

    /// <summary>
    /// プレイヤーの向きの更新
    /// </summary>
    void UpdatePlayerDirection()
    {
        switch (xPositionStatus)
        {
            case XPositionStatus.left:
                playerAnimator.GetComponent<Transform>().localScale = new Vector3(-1f, 1f, 1f);
                break;
            case XPositionStatus.right:
                playerAnimator.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
                break;
        }
    }

    /// <summary>
    /// プレイヤー動き処理の更新
    /// </summary>
    void UpdateMovement()
    {
        CheckOnMushroom();

        if (knockBackForce != Vector2.zero)
        {
            Knockback();
        }
        else
        {
            Movement();
        }
    }

    private void Movement()
    {
        float velocity_x = isCliming ? 0f : Run();
        float velocity_y = isCliming ? Climb() : isMushroomBounce ? MushroomBounce() : Jump();

        // 壁にへばり付いてる時
        if (isGripWall)
        {
            velocity_y = playerGravity - grippingPower;
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

        //playerRg2d.velocity = new Vector2(velocity_x, velocity_y) + platformVelocity;
        playerVelocity.x = velocity_x;
        playerVelocity.y = velocity_y;
        playerRg2d.velocity = GetMergedMovePlatformVelocity(playerVelocity);
    }

    private void Knockback()
    {
        transform.Translate(knockBackForce * Time.fixedDeltaTime, Space.Self);
    }

    private Vector2 GetMergedMovePlatformVelocity(Vector2 originVelocity)
    {
        Vector2 resVelocity = originVelocity;
        var standOnObj = gm.standOnObj;
        if (standOnObj != null && standOnObj.GetComponent<Platform>() != null)
        {
            resVelocity += standOnObj.GetComponent<Platform>().SelfVelocity;
        }
        return resVelocity;
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
                _playerRunSpeed = playerRunSpeed;
                break;
            case XPositionStatus.left:
                _playerRunSpeed = -playerRunSpeed;
                break;
        }

        if (xPositionStatus == XPositionStatus.right || xPositionStatus == XPositionStatus.left)
        {
            playerAnimator.SetBool("run", true);
            dushTime += Time.deltaTime;
        }
        else
        {
            _playerRunSpeed = 0f;
            playerAnimator.SetBool("run", false);
            dushTime = 0f;
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
        float _playerJumpSpeed = groundCheck.IsInGround ? 0f : -playerGravity; ;

        if (isJump)
        {
            canJumpHeight = playerJumpPos + jumpLimitHight > transform.position.y;
            bool isCanJumpTime = playerJumpLimitTime > playerJumpTime;

            if (canJumpHeight && isCanJumpTime && !headCheck.IsInGround)
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
    /// ハシゴ登る
    /// </summary>
    /// <returns></returns>
    float Climb()
    {
        float climbSpeed = 0f;
        if (isCliming)
        {
            switch (climbType)
            {
                case e_ClimbType.climbUp:
                    if (canClimbUp)
                    {
                        climbSpeed = playerClimbSpeed;
                        playerAnimator.SetFloat("climbSpeed", 1f);
                        playerAnimator.SetBool("climb", true);
                    }
                    else
                    {
                        climbSpeed = 0f;
                        isCliming = false;
                        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ground"), LayerMask.NameToLayer("Player"), false);
                    }
                    break;
                case e_ClimbType.climbDown:
                    if (canClimbDown)
                    {
                        climbSpeed = -playerClimbSpeed;
                        playerAnimator.SetFloat("climbSpeed", 1f);
                        playerAnimator.SetBool("climb", true);
                    }
                    else
                    {
                        climbSpeed = 0f;
                        isCliming = false;
                        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ground"), LayerMask.NameToLayer("Player"), false);
                    }
                    break;
                case e_ClimbType.none:
                    playerAnimator.SetFloat("climbSpeed", 0f);
                    break;
            }
        }

        return climbSpeed;
    }

    void CheckOnMushroom()
    {
        if (groundCheck.IsInMushroom)
        {
            isJump = false;
            isMushroomBounce = true;
            playerJumpPos = transform.position.y;
            playerJumpTime = 0f;
        }
    }

    bool isMushroomBounce = false;
    float MushroomBounce()
    {
        float _playerJumpSpeed = -playerGravity;

        if (isMushroomBounce)
        {
            canJumpHeight = playerJumpPos + 3.5f > transform.position.y;
            bool canTime = playerJumpLimitTime > playerJumpTime;

            if (canJumpHeight && canTime && !headCheck.IsInGround)
            {
                _playerJumpSpeed = playerMushroomJumpSpeed;
                playerJumpTime += Time.deltaTime;
            }
            else
            {
                isMushroomBounce = false;
            }

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
    public bool CheckCollisionDetectionWithEnemy(Transform enemyTrans)
    {
        bool isAttectedByEnemy = false;
        float playerHeight = playerCollider.size.y;

        // 踏みつける判定の高さ
        float stepOnHeight = playerHeight * (stepOnRate / 100f);
        float stepOnPos = transform.position.y - playerHeight / 2 + stepOnHeight;

        if (enemyTrans.position.y < stepOnPos && !groundCheck.IsInGround && !isCliming)
        {
            isAttectedByEnemy = false;
        }
        else
        {
            isAttectedByEnemy = true;
        }
        return isAttectedByEnemy;
    }


    /// <summary>
    /// 当たり判定処理
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.transform.tag)
        {
            case GameConfig.SpikeTag:
                OnDamage(collision.transform.position);
                break;
        }
    }

    /// <summary>
    /// 当たり判定処理
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        switch (collision.transform.tag)
        {
            case GameConfig.SpikeTag:
                OnDamage(collision.transform.position);
                break;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnTriggerTouched(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        OnTriggerTouched(collision);
    }

    Door door;
    void OnTriggerTouched(Collider2D collision)
    {
        switch (collision.tag)
        {
            case GameConfig.WallTag:
                CheckCanKickWall(collision);
                break;
            case GameConfig.ladderTag:
                CheckCanClimbLadder(collision);
                break;
            case GameConfig.TreasureTag:
                TouchingTreasure = collision.GetComponent<Treasure>();
                break;
            case GameConfig.DoorTag:
                door = collision.GetComponent<Door>();
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case GameConfig.WallTag:
                isGripWall = false;
                playerAnimator.SetBool("wallj", false);
                wallDirection = e_WallDirection.none;
                break;
            case GameConfig.ladderTag:
                canClimbUp = false;
                canClimbDown = false;
                isCliming = false;
                ladderTopPos = 0f;
                ladderBottomPos = 0f;
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ground"), LayerMask.NameToLayer("Player"), false);
                playerAnimator.SetFloat("climbSpeed", 1f);
                playerAnimator.SetBool("climb", false);
                break;
            case GameConfig.TreasureTag:
                TouchingTreasure = null;
                break;
            case GameConfig.DoorTag:
                door = null;
                break;
        }
    }

    /// <summary>
    /// 壁キック可能かをチェック
    /// </summary>
    private void CheckCanKickWall(Collider2D collision)
    {
        if (collision.tag == GameConfig.WallTag)
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
    /// ハシゴを登れるかチェック
    /// </summary>
    /// <param name="collision"></param>
    private void CheckCanClimbLadder(Collider2D collision)
    {
        if (ladderTopPos == 0f && ladderBottomPos == 0f)
        {
            ladderCenter = collision.GetComponent<BoxCollider2D>().bounds.center;
            ladderExtents = collision.GetComponent<BoxCollider2D>().bounds.extents;
            ladderTopPos = ladderCenter.y + ladderExtents.y;
            ladderBottomPos = ladderCenter.y - ladderExtents.y;
        }
        playerBottomPos = playerCollider.bounds.center.y - playerCollider.bounds.extents.y;
        canClimbUp = ladderTopPos - 0.1f > playerBottomPos;
        canClimbDown = ladderBottomPos + 0.1f < playerBottomPos;

        if (isCliming)
        {
            transform.position = new Vector2(ladderCenter.x, transform.position.y);
        }
    }

    /// <summary>
    /// ダメージ受けた時
    /// </summary>
    public void OnDamage(Vector2 damagePos)
    {
        // 無敵期間中
        if (IsInvincible) return;

        // 死んだら何もしない
        if (isDie) return;

        // ゲームクリアしたら何もしない
        if (gm.IsGameClear) return;

        if (gm.PlayerCurrentHp > 0)
        {
            // HPを１減らす
            gm.PlayerCurrentHp--;

            // PlayerHit
            playerAnimator.Play("PlayerHit");
        }

        if (gm.PlayerCurrentHp <= 0)
        {
            // 死んだ時
            isDie = true;
            GameOverTask();
        }
        else
        {
            // 無敵
            StartCoroutine(DoInvincibleTime());
            StartCoroutine(ComputeKnockbackPos(damagePos));
        }
    }

    /// <summary>
    /// プレイヤーの初期状態の初期化
    /// </summary>
    private void InitPlayerStatus ()
    {
        isDie = false;
        isJump = false;
        isCliming = false;

        playerRg2d.velocity = new Vector2(0f, 0f);
        playerAnimator.SetBool("run", false);
        playerAnimator.SetBool("jump", false);
        playerAnimator.SetFloat("climbSpeed", 1f);
        playerAnimator.SetBool("climb", false);
        playerAnimator.SetBool("ground", true);
        playerAnimator.Play("PlayerIddle");
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ground"), LayerMask.NameToLayer("Player"), false);
    }

    /// <summary>
    /// 攻撃された時の無敵時間
    /// </summary>
    /// <returns></returns>
    public IEnumerator DoInvincibleTime()
    {
        IsInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        IsInvincible = false;

        // 無敵時間が過ぎたらPlayer.Rigidbody2DのSleepを解除する
        playerRg2d.WakeUp();
    }

    private IEnumerator ComputeKnockbackPos(Vector3 damagePos)
    {
        knockBackForce = DamageCausedKnockbackForce;
        Vector2 relativePosition = transform.position - damagePos;
        knockBackForce.x *= Mathf.Sign(relativePosition.x);
        yield return new WaitForSeconds(0.3f);
        knockBackForce = Vector2.zero;
    }

    private void UpdateInvincibleInfo()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"), IsInvincible);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Spike"), LayerMask.NameToLayer("Player"), IsInvincible);
        playerAnimator.SetBool("invincible", IsInvincible);
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
        //RunOperationUsePhone();
        //ClimbOperationUsePhone();
        JumpOperationUsePhone();
        
    }

    /// <summary>
    /// スマホによる横移動(TODO　廃止)
    /// </summary>
    // xPositionStatusOverriteは進行方向を保持するため
    XPositionStatus xPositionStatusOverrite = XPositionStatus.none;
    private void RunOperationUsePhone()
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
    private void JumpOperationUsePhone()
    {
        if (Application.isEditor) return;
        
        if (Input.touchCount > 0)
        {
            if (groundCheck.IsInGround || isGripWall)
            {
                TouchType touchType = TouchType.jumpTouch;
                Touch touch = GetTouchInfo(touchType);

                // UIボタン類をタップした時に何もしない
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId)) return;

                if (touch.position.x > Screen.width / 2)
                {
                    // fingerIdを記録しておく
                    fingerIdDic[touchType] = touch.fingerId;
                    if (touch.phase == TouchPhase.Began)
                    {
                        DoJump(playerJumpLimitHight);
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
    /// スマホによるハシゴ操作（TODO 廃止）
    /// </summary>
    private void ClimbOperationUsePhone()
    {
        if (Input.touchCount > 0)
        {
            TouchType touchType = TouchType.runTouch;
            Touch touch = GetTouchInfo(touchType);

            if (touch.position.x < Screen.width / 2)
            {
                // fingerIdを記録しておく
                fingerIdDic[touchType] = touch.fingerId;

                if (canClimbUp)
                {
                    if (touch.deltaPosition.y > 10f)
                    {
                        isCliming = true;
                        isJump = false;
                        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ground"), LayerMask.NameToLayer("Player"), true);
                        climbType = e_ClimbType.climbUp;
                    }
                }

                if (canClimbDown)
                {
                    if (touch.deltaPosition.y < -10f)
                    {
                        isCliming = true;
                        isJump = false;
                        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ground"), LayerMask.NameToLayer("Player"), true);
                        climbType = e_ClimbType.climbDown;
                    }
                }

                if (isCliming)
                {
                    switch (touch.phase)
                    {
                        case TouchPhase.Ended:
                            climbType = e_ClimbType.none;
                            break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// キーボードによる動きの操作（Run、Jump）
    /// </summary>
    private void DoMovementOperationByKeyborad()
    {
        if (!Application.isEditor) return;
        RunOperationUseKeyborad();
        JumpOperationUseKeyborad();
        ClimbOperationUseKeyborad();
        FallDownOperationUseKeyborad();
    }

    /// <summary>
    /// キーボードによる移動操作
    /// </summary>
    private void RunOperationUseKeyborad()
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
    private void JumpOperationUseKeyborad()
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
    /// キーボードによるハシゴ操作
    /// </summary>
    private void ClimbOperationUseKeyborad()
    {
        if (canClimbUp)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                isCliming = true;
                isJump = false;
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ground"), LayerMask.NameToLayer("Player"), true);
            }
        }

        if (canClimbDown)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                isCliming = true;
                isJump = false;
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ground"), LayerMask.NameToLayer("Player"), true);
            }
        }

        if (isCliming)
        {
            if (Input.GetAxis("Vertical") > 0f)
            {
                climbType = e_ClimbType.climbUp;
            }
            else if (Input.GetAxis("Vertical") < 0f)
            {
                climbType = e_ClimbType.climbDown;
            }
            else
            {
                climbType = e_ClimbType.none;
            }
        }
    }

    bool isFallDown = false;
    private void FallDownOneWayPlatform()
    {
        GameObject standOnObj = gm.standOnObj;
        if (isCliming
            || standOnObj == null || standOnObj != null && LayerMask.LayerToName(standOnObj.layer) != GameConfig.OneWayPlatformLayer)
        {
            isFallDown = false;
            return;
        }

        if (isFallDown)
        {
            isFallDown = false;
            StartCoroutine(IgnoreCollision(GameConfig.OneWayPlatformLayer, "Player", 0.1f));
        }
    }

    private void FallDownOperationUseKeyborad()
    {
        // 下キー押したらOneWayPlatformのフラグをtrueにする、実際にできるかどうかは実行時に判断している
        if (Input.GetAxis("Vertical") < 0f) isFallDown = true;
    }

    /// <summary>
    /// 上キー押下によるジャンプ
    /// </summary>
    private void OnUpArrowDown()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (!canClimbUp && (groundCheck.IsInGround || isGripWall))
            {
                DoJump(playerJumpLimitHight);
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
            if (!canClimbUp && (groundCheck.IsInGround || isGripWall))
            {
                DoJump(playerJumpLimitHight);
            }
        }
        else
        {
            isJump = false;
        }
    }

    private void DoJump(float jumpLimitHight)
    {
        if (isCliming) return;

        // jumpのSE
        SoundManager.Instance.Play(jumpSe);

        isJump = true;
        playerJumpPos = transform.position.y;
        playerJumpTime = 0f;
        this.jumpLimitHight = jumpLimitHight;

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

    // 敵を踏みつけた時にバウンド
    public void OnBound(float boundHeight)
    {
        DoJump(boundHeight);
    }

    public void ShowDust()
    {
        if (!isJump && !isCliming && !isGripWall && !isDie)
        {
            var _dust = Instantiate(dust);
            _dust.transform.position = dust.transform.position;
            _dust.gameObject.SetActive(true);
        }
    }

    private void GameOverTask()
    {
        gm.cinemachineCamera.Follow = null;
        playerCollider.enabled = false;
        Sleep();
        iTween.MoveAdd(gameObject, iTween.Hash("y", 0.3f, "time", 0.5f, "easeType", iTween.EaseType.easeOutBounce));
        iTween.MoveAdd(gameObject, iTween.Hash("y", -10f, "time", 5f, "delay", 0.5f));
    }

    public void Sleep()
    {
        xPositionStatus = XPositionStatus.none;
        playerRg2d.velocity = Vector2.zero;
        playerRg2d.Sleep();
        playerAnimator.Play("PlayerIddle");
    }

    public void WakeUp()
    {
        playerRg2d.WakeUp();
    }

    public void OnLeftButtonDown()
    {
        if (GameUtility.Instance.IsGamePause) return;
        xPositionStatus = XPositionStatus.left;
    }

    public void OnLeftButtonUp()
    {
        if (GameUtility.Instance.IsGamePause) return;
        xPositionStatus = XPositionStatus.none;
    }

    public void OnRightButtonDown()
    {
        if (GameUtility.Instance.IsGamePause) return;
        xPositionStatus = XPositionStatus.right;
    }

    public void OnRightButtonUp()
    {
        if (GameUtility.Instance.IsGamePause) return;
        xPositionStatus = XPositionStatus.none;
    }

    public void OnUpButtonDown()
    {
        if (GameUtility.Instance.IsGamePause) return;
        if (canClimbUp)
        {
            isCliming = true;
            isJump = false;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ground"), LayerMask.NameToLayer("Player"), true);
            climbType = e_ClimbType.climbUp;
        }

        if (door != null)
        {
            door.DoAnime(
                true,
                () =>
                {
                    switch (gm.CurrentGameMode)
                    {
                        case e_GameMode.Title:
                            gm.LoadSceneTo(e_SceneName.StageSelection.ToString());
                            break;
                        case e_GameMode.Normal:
                            gm.stageUiView.SwitchOnBlackMask();
                            break;
                    }
                }
            );
        }
    }

    public void OnUpButtonUp()
    {
        if (GameUtility.Instance.IsGamePause) return;
        climbType = e_ClimbType.none;
    }

    public void OnDownButtonDown()
    {
        if (GameUtility.Instance.IsGamePause) return;
        if (canClimbDown)
        {
            isCliming = true;
            isJump = false;
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Ground"), LayerMask.NameToLayer("Player"), true);
            climbType = e_ClimbType.climbDown;
        }

        // 下キー押したらOneWayPlatformのフラグをtrueにする、実際にできるかどうかは実行時に判断している
        isFallDown = true;
    }

    public void OnDownButtonUp()
    {
        if (GameUtility.Instance.IsGamePause) return;
        climbType = e_ClimbType.none;
    }

    private IEnumerator IgnoreCollision(string layerName1, string layerName2, float time)
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(layerName1), LayerMask.NameToLayer(layerName2), true);
        yield return new WaitForSeconds(time);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer(layerName1), LayerMask.NameToLayer(layerName2), false);
    }
}
