using System.Collections;
using UnityEngine;

public class SlimeEnemy : Enemy
{
    private bool isRight;

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

            //if (!base.IsCanMove) yield break;

            if (base.sr.isVisible)
            {
                if (base.ecc != null && base.ecc.IsOn)
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
