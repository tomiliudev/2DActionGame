using System.Collections;
using UnityEngine;

public class BatEnemy : Enemy
{
    private const string playerTag = "Player";
    private Vector2 playerPostion;
    private Vector2 selfPosition;

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

            // 敵が死んだら何もしない
            if (base.IsEnemyDead()) yield break;

            // ゲームクリアしたら何もしない
            if (base.IsGameClear)
            {
                base.rb2D.velocity = Vector2.zero;
                yield break;
            }

            if (base.sr.isVisible)
            {
                if(base.animator != null) base.animator.SetBool("isGo", true);
                playerPostion = GameObject.FindWithTag(playerTag).transform.position;
                selfPosition = transform.localPosition;
                var playerVector = playerPostion - selfPosition;

                yield return new WaitForFixedUpdate();
                base.rb2D.velocity = playerVector;
            }
            else
            {
                base.rb2D.Sleep();
            }
        }
    }
}
