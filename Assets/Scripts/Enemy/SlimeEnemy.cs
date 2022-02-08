using System.Collections;
using UnityEngine;

public sealed class SlimeEnemy : Enemy
{
    [SerializeField] GroundCheck groundCheck;
    [SerializeField] GroundCheck headCheck;

    private bool isJump = false;
    private float jumpSpeed = -3f;
    private float jumpForce = 10f;
    private float jumpLimitHeight = 0f;
    private const float JumpLimitHeightMax = 2f;
    private float[] jumpLimitHeightValue = new float[] { 1f, 1.5f, JumpLimitHeightMax };
    private float jumpPos;
    private float[] lateralMoveValue = new float[] { -3f, 0f, 3f };
    private float lateralMoveSpeed = 0f;
    private bool isPlayerHit;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Move());
    }

    /// <summary>
    /// スライムの移動
    /// </summary>
    /// <returns></returns>
    IEnumerator Move()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (base.IsDead) yield break;

            if (base.sr.isVisible)
            {
                isPlayerHit = base.IsHitPlayer();

                if (groundCheck.IsInGround)
                {
                    // 移動方向を決める
                    GetLateralMoveSpeed();

                    // プレイヤーが監視範囲内に来たら
                    if (isPlayerHit)
                    {
                        isJump = true;
                        jumpPos = transform.position.y;
                        jumpLimitHeight = JumpLimitHeightMax;
                    }
                    // 常にジャンプではなく、一定の確率でジャンプする
                    else if (Random.Range(1, 100) > 95)
                    {
                        isJump = true;
                        jumpPos = transform.position.y;
                        jumpLimitHeight = Random.Range(0, jumpLimitHeightValue.Length);
                    }
                    else
                    {
                        isJump = false;
                        jumpSpeed = 0f;
                    }
                }
                else if (!isJump)
                {
                    jumpSpeed = -jumpForce;
                }

                if (isJump)
                {
                    jumpSpeed = jumpForce;
                    bool isCanHeight = transform.position.y < jumpPos + jumpLimitHeight;
                    if (!isCanHeight || headCheck.IsInGround)
                    {
                        jumpSpeed = -jumpForce;
                        isJump = false;
                    }
                }

                rb2D.velocity = new Vector2(lateralMoveSpeed, jumpSpeed);
            }
            else
            {
                rb2D.Sleep();
            }
        }
    }

    /// <summary>
    /// 移動方向
    /// </summary>
    private void GetLateralMoveSpeed()
    {
        if (isPlayerHit)
        {
            lateralMoveSpeed = base.playerVector.x > 0f ? 3f : -3f;
        }
        else if (Random.Range(1, 100) > 90)
        {
            // 移動の向き変更
            lateralMoveSpeed = lateralMoveValue[Random.Range(0, lateralMoveValue.Length)];
        }

        if (lateralMoveSpeed < 0)
        {
            transform.localScale = new Vector2(1f, transform.localScale.y);
        }
        if (lateralMoveSpeed > 0)
        {
            transform.localScale = new Vector2(-1f, transform.localScale.y);
        }
    }
}
