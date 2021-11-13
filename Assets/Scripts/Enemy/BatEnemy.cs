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
            // 敵が死んだら何もしない
            if (base.IsEnemyDead()) yield break;

            // ゲームクリアしたら何もしない
            if (base.IsGameClear)
            {
                base.rb2D.velocity = Vector2.zero;
                yield break;
            }

            yield return new WaitForFixedUpdate();

            if (base.sr.isVisible)
            {
                playerPostion = playerTransform.position;
                selfPosition = transform.position;
                var playerVector = playerPostion - selfPosition;
                var hit = Physics2D.Raycast(transform.position, playerVector, 5f, LayerMask.GetMask("Player"));

                if (hit)
                {
                    Debug.Log(hit.transform.gameObject.name);
                    if (base.animator != null) base.animator.SetBool("isGo", true);
                    base.rb2D.velocity = playerVector;
                }
            }
            else
            {
                base.rb2D.Sleep();
            }
        }
    }
}
