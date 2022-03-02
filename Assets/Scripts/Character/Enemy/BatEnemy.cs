using System.Collections;
using UnityEngine;

public sealed class BatEnemy : Enemy
{
    private bool isFollowPlayer;

    // 一時停止
    bool isAttackPlayer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FollowPlayer());
    }

    /// <summary>
    /// プレイヤーを追従する
    /// </summary>
    /// <returns></returns>
    IEnumerator FollowPlayer()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (base.IsDead) yield break;

            // 一時停止
            if (base.IsDoFreeze(isAttackPlayer)) continue;

            if (base.sr.isVisible)
            {
                GameObject hitObj = base.GetHitClosestObj();
                if (!isFollowPlayer
                    && hitObj != null && hitObj.name == GameConfig.PlayerName)
                {
                    isFollowPlayer = true;
                }

                if (isFollowPlayer)
                {
                    if (base.animator != null) base.animator.SetBool("isGo", true);
                    base.rb2D.velocity = base.playerVector.normalized * base.moveSpeed * Time.fixedDeltaTime;

                    if (base.playerVector.x < 0f)
                    {
                        transform.localScale = new Vector2(-1f, transform.localScale.y);
                    }
                    else
                    {
                        transform.localScale = new Vector2(1f, transform.localScale.y);
                    }
                }
            }
            else
            {
                base.rb2D.Sleep();
            }
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.collider.tag == GameConfig.PlayerTag)
        {
            if (base.gm.player.CheckCollisionDetectionWithEnemy(transform))
            {
                // プレイヤーを攻撃した場合、一時フリーズに入る
                StartCoroutine(DoFreeze());
            }
        }
    }

    // 一時フリーズ
    IEnumerator DoFreeze()
    {
        isAttackPlayer = true;

        var x = transform.position.x;
        var y = transform.position.y;

        Hashtable hash = new Hashtable();
        hash.Add("x", x + (-playerVector.normalized.x));
        hash.Add("y", y + (-playerVector.normalized.y));
        hash.Add("time", 0.5f);
        iTween.MoveTo(gameObject, hash);

        yield return new WaitForSeconds(2f);
        isAttackPlayer = false;
    }
}
