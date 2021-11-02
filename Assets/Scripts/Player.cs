using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    [SerializeField] Rigidbody2D playerRg2d;
    [SerializeField] float playerRunSpeed;
    [SerializeField] GroundCheck groundCheck;
    [SerializeField] AnimationCurve playerRunCurve;

    [SerializeField] float playerGravity;
    [SerializeField] float playerJumpSpeed;
    [SerializeField] float playerJumpLimitHight;
    [SerializeField] float playerJumpLimitTime;
    [SerializeField] GroundCheck headCheck;
    [SerializeField] AnimationCurve playerJumpCurve;

    enum XPositionStatus
    {
        none,
        right,
        left
    };
    XPositionStatus xPositionStatus = XPositionStatus.none;
    XPositionStatus beforeXPositionStatus = XPositionStatus.none;

    private float _playerRunSpeed = 0f;
    private float dushTime;

    private bool isJump = false;
    private float playerJumpPos;
    private float playerJumpTime;

    // Start is called before the first frame update
    void Start()
    {
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
        Jump_SmartPhoneVersion();
    }

    void FixedUpdate()
    {
        playerAnimator.SetBool("jump", isJump);
        playerAnimator.SetBool("ground", groundCheck.IsInGround);

        if (Application.isEditor)
        {
            playerRg2d.velocity = new Vector2(Run(), Jump());
        }
        else
        {
            playerRg2d.velocity = new Vector2(Run_SmartPhoneVersion(), playerRg2d.velocity.y);
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
        if (Input.touchCount <= 0)
        {
            OnRunFinish();
            return 0f;
        }

        Touch touch = Input.GetTouch(0);

        if (touch.position.x > Screen.width / 2)
        {
            OnRunFinish();
            return 0f;
        }

        switch (touch.phase)
        {
            case TouchPhase.Ended:
                OnRunFinish();
                break;
        }

        if(touch.deltaPosition.x > 1f)
        {
            xPositionStatus = XPositionStatus.right;
        }else if (touch.deltaPosition.x < -1f)
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

    void Jump_SmartPhoneVersion()
    {
        int touchIndex = Input.touchCount - 1;
        if (touchIndex < 0) return;

        Touch touch = Input.GetTouch(touchIndex);

        if (touch.position.x < Screen.width / 2)
        {
            return;
        }

        if (touch.phase == TouchPhase.Began && groundCheck.IsInGround)
        {
            playerRg2d.AddForce(transform.up * 1000f);
        }
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
        else if(isJump)
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
}
