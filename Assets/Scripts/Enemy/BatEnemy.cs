using System.Collections;
using UnityEngine;

public class BatEnemy : Enemy
{
    private const string playerTag = "Player";
    private Vector2 playerPostion;
    private Vector2 selfPosition;

    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindWithTag(playerTag).transform;
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
                playerPostion = playerTransform.position;
                selfPosition = transform.position;
                var playerVector = playerPostion - selfPosition;
                var hit = Physics2D.Raycast(transform.position, playerVector, 5f, LayerMask.GetMask("Player"));

                if (hit)
                {
                    if (base.animator != null) base.animator.SetBool("isGo", true);
                    base.rb2D.velocity = playerVector;

                    if (playerVector.x < 0f)
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
