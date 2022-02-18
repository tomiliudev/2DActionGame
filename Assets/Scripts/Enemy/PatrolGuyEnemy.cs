using System.Collections;
using UnityEngine;

public sealed class PatrolGuyEnemy : Enemy
{
    Vector2 rayStart;
    private bool isRight;

    private int groundLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        rayStart = Vector2.zero;
        groundLayerMask = 1 << LayerMask.NameToLayer("WeakBlock") | 1 << LayerMask.NameToLayer("Ground");
        StartCoroutine(Move());
    }

    /// <summary>
    /// プレイヤーを追従する
    /// </summary>
    IEnumerator Move()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            yield return new WaitForFixedUpdate();

            if (base.IsDead) yield break;

            // 一時停止
            if (base.IsDoFreeze()) continue;

            rayStart = transform.position;
            if (isRight)
            {
                rayStart += new Vector2(0.2f, 0f);
            }
            else
            {
                rayStart -= new Vector2(0.2f, 0f);
            }
            var hit_ground = Physics2D.Raycast(rayStart, Vector2.down, 1f, groundLayerMask);

            if (base.wallCollisionCheck != null && base.wallCollisionCheck.IsOn
                || hit_ground.collider == null
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
            rb2D.velocity = new Vector2(xVector * moveSpeed * Time.fixedDeltaTime, -gravity);
        }
    }
}
