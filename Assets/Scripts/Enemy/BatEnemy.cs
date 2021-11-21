using System.Collections;
using UnityEngine;

public class BatEnemy : Enemy
{
    private bool isFollowPlayer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FollowPlayer());
    }

    private void Update()
    {
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

            if (!base.IsCanMove) yield break;

            if (base.sr.isVisible)
            {
                if (!isFollowPlayer && base.GetPlayerHit())
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
}
