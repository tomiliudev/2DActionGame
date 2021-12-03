using System.Collections;
using UnityEngine;

public class PatrolGuyEnemy : Enemy
{
    private bool isRight;

    private int groundLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        groundLayerMask = 1 << LayerMask.NameToLayer("WeakBlock") | 1 << LayerMask.NameToLayer("Ground");
        StartCoroutine(Move());
    }

    /// <summary>
    /// プレイヤーを追従する
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
                var hit_weakBlock = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, groundLayerMask);

                if (base.wallCollisionCheck != null && base.wallCollisionCheck.IsOn
                    || hit_weakBlock.collider == null
                )
                {
                    isRight = !isRight;
                }

                float xVector = -1f;
                if (isRight)
                {
                    xVector = 1f;
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
                else
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                rb2D.velocity = new Vector2(xVector * moveSpeed, -gravity);
            }
            else
            {
                rb2D.Sleep();
            }
        }
    }
}
