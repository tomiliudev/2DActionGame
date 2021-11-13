using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField] int playerMaxHp;
    [SerializeField] GameObject heartPrefab;
    [SerializeField] Transform heartBar;

    [SerializeField] Text text1;
    [SerializeField] Text text2;
    [SerializeField] Text text3;
    [SerializeField] Text text4;
    [SerializeField] Text text5;
    [SerializeField] Text text6;
    [SerializeField] Text text7;
    [SerializeField] Text text8;
    [SerializeField] Text text9;

    public bool IsGameClear { get; set; }

    enum XPositionStatus
    {
        none,
        right,
        left
    };
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
    private bool isJump = false;
    private float playerJumpPos;
    private float playerJumpTime;

    private int playerHp;
    public int PlayerHp { get { return playerHp; } }
    private GameObject[] playerHpPrefabs;

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
        // プレイヤー状態の初期化
        InitPlayerStatus();
        InitPlayerHp();

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
        if(Input.touchCount <= 0)
        {
            // 画面に何もタッチしていない時にすべて-1でリセットする
            List<TouchType> keys = fingerIdDic.Keys.ToList();
            foreach(TouchType key in keys)
            {
                fingerIdDic[key] = -1;
            }
        }

        if (!IsGameClear)
        {
            // ゲームクリアしてないなら操作可能
            //Jump_SmartPhoneVersion();
        }
        else
        {
            // プレイヤー状態の初期化
            InitPlayerStatus();
        }
    }

    void FixedUpdate()
    {
        playerAnimator.SetBool("jump", isJump);
        playerAnimator.SetBool("ground", groundCheck.IsInGround);

        if (!IsGameClear)
        {
            if (Application.isEditor)
            {
                playerRg2d.velocity = new Vector2(Run(), Jump());
            }
            else
            {
                //playerRg2d.velocity = new Vector2(Run_SmartPhoneVersion(), playerRg2d.velocity.y);
                playerRg2d.velocity = new Vector2(Run_SmartPhoneVersion(), Jump_SmartPhoneVersion2());
            }
        }
    }

    void OnRunFinish()
    {
        playerAnimator.SetBool("run", false);
        _playerRunSpeed = 0f;
        dushTime = 0f;
        xPositionStatus = XPositionStatus.none;
        beforeXPositionStatus = XPositionStatus.none;
    }

    float Run_SmartPhoneVersion()
    {
        int touchIndex = Input.touchCount - 1;
        if (touchIndex < 0)
        {
            OnRunFinish();
            return 0f;
        }

        TouchType touchType = TouchType.runTouch;
        Touch touch = GetTouchInfo(touchType);

        if (touch.position.x > Screen.width / 2)
        {
            OnRunFinish();
            return 0f;
        }

        // fingerIdを記録しておく
        fingerIdDic[touchType] = touch.fingerId;

        switch (touch.phase)
        {
            case TouchPhase.Ended:
                OnRunFinish();
                break;
        }

        if (touch.deltaPosition.x > 1f)
        {
            xPositionStatus = XPositionStatus.right;
        }
        else if (touch.deltaPosition.x < -1f)
        {
            xPositionStatus = XPositionStatus.left;
        }

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
        }

        if (beforeXPositionStatus != XPositionStatus.none && xPositionStatus != XPositionStatus.none && beforeXPositionStatus != xPositionStatus)
        {
            dushTime = 0f;
        }
        beforeXPositionStatus = xPositionStatus;

        _playerRunSpeed *= playerRunCurve.Evaluate(dushTime);

        return _playerRunSpeed;
    }

    float Run()
    {
        var horizontalKey = Input.GetAxis("Horizontal");
        if (horizontalKey > 0)
        {
            xPositionStatus = XPositionStatus.right;
            playerAnimator.GetComponent<Transform>().localScale = new Vector3(1f, 1f, 1f);
            playerAnimator.SetBool("run", true);
            _playerRunSpeed = playerRunSpeed;
            dushTime += Time.deltaTime;
        }
        else if (horizontalKey < 0)
        {
            xPositionStatus = XPositionStatus.left;
            playerAnimator.GetComponent<Transform>().localScale = new Vector3(-1f, 1f, 1f);
            playerAnimator.SetBool("run", true);
            _playerRunSpeed = -playerRunSpeed;
            dushTime += Time.deltaTime;
        }
        else
        {
            playerAnimator.SetBool("run", false);
            _playerRunSpeed = 0f;
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

    #region AddForce Jump
    /// <summary>
    /// AddForceをつかったジャンプ
    /// 問題点：高くジャンプすると床のコライダーを突き抜けてしまう現象が発生する
    /// </summary>
    void Jump_SmartPhoneVersion()
    {
        int touchIndex = Input.touchCount - 1;
        if (touchIndex < 0) return;

        TouchType touchType = TouchType.jumpTouch;
        Touch touch = GetTouchInfo(touchType);

        if (touch.position.x < Screen.width / 2)
        {
            return;
        }

        // fingerIdを記録しておく
        fingerIdDic[touchType] = touch.fingerId;

        switch (touch.phase)
        {
            case TouchPhase.Began:
                if (groundCheck.IsInGround)
                {
                    playerRg2d.AddForce(transform.up * 1000f);
                }
                break;
            case TouchPhase.Ended:
                break;
        }
    }
    #endregion

    /// <summary>
    /// rigidbody.velocityを使ったジャンプ
    /// </summary>
    /// <returns></returns>
    float Jump_SmartPhoneVersion2()
    {
        float _playerJumpSpeed = -playerGravity;

        if (groundCheck.IsInGround && Input.touchCount > 0)
        {
            TouchType touchType = TouchType.jumpTouch;
            Touch touch = GetTouchInfo(touchType);

            if (touch.position.x > Screen.width / 2)
            {
                // fingerIdを記録しておく
                fingerIdDic[touchType] = touch.fingerId;

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        isJump = true;
                        _playerJumpSpeed = playerJumpSpeed;
                        playerJumpPos = transform.position.y;
                        playerJumpTime = 0f;
                        break;
                    case TouchPhase.Ended:
                        break;
                }
            }
        }

        if (isJump)
        {
            bool canHight = playerJumpPos + playerJumpLimitHight > transform.position.y;
            bool canTime = playerJumpLimitTime > playerJumpTime;

            if (canHight && canTime && !headCheck.IsInGround)
            {
                _playerJumpSpeed = playerJumpSpeed;
                playerJumpTime += Time.deltaTime;
            }
            else
            {
                isJump = false;
            }

            _playerJumpSpeed *= playerJumpCurve.Evaluate(playerJumpTime);
        }

        return _playerJumpSpeed;
    }

    float Jump()
    {
        float _playerJumpSpeed = -playerGravity;
        var verticalKey = Input.GetAxis("Vertical");

        if (groundCheck.IsInGround)
        {
            if (verticalKey > 0f)
            {
                isJump = true;
                _playerJumpSpeed = playerJumpSpeed;
                playerJumpPos = transform.position.y;
                playerJumpTime = 0f;
            }
            else
            {
                isJump = false;
            }
        }
        else if (isJump)
        {
            bool canHight = playerJumpPos + playerJumpLimitHight > transform.position.y;
            bool canTime = playerJumpLimitTime > playerJumpTime;

            if (verticalKey > 0f && canHight && canTime && !headCheck.IsInGround)
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
        if (IsGameClear) return;

        if (collision.gameObject.tag == "Enemy")
        {
            if (playerHp > 0)
            {
                // まだ生きてる時

                // 無敵時間
                StartCoroutine(DoInvincibleTime());

                // HPバー
                DoHpBarAnimation(-1);

                // PlayerHit
                playerAnimator.Play("PlayerHit");
            }

            if (playerHp <= 0)
            {
                // 死んだ時
                isDie = true;
            }
        }
    }

    private void InitPlayerHp ()
    {
        // プレイヤーのHP
        playerHp = playerMaxHp;
        playerHpPrefabs = new GameObject[playerMaxHp];
        for (int i = 0; i < playerMaxHp; i++)
        {
            playerHpPrefabs[i] = Instantiate(heartPrefab, heartBar);
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
    /// プレイヤーHPの増減を制御する
    /// </summary>
    /// <param name="hp">例：-3では、HP３個減る</param>
    public void DoHpBarAnimation(int hp)
    {
        for (int i = 0; i < Mathf.Abs(hp); i++)
        {
            if (hp < 0)
            {
                playerHp--;
                if (playerHp < 0) playerHp = 0;
                playerHpPrefabs[playerHp].GetComponent<Animator>().Play("PlayerHpHit");
            }
            else
            {
                playerHp++;
                if (playerHp > playerMaxHp) playerHp = playerMaxHp;
            }
        }
    }
}