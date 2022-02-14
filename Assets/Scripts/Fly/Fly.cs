using System.Collections;
using UnityEngine;

public sealed class Fly : Enemy
{
    private Vector3 originPos;
    private Vector3 originMovePos;
    private Vector3 playerMovePos;

    const float MaxChangeValue = 1f;

    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        StartCoroutine(UpdateOriginMovePos());
        StartCoroutine(UpdatePlayerMovePos());
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (base.IsHitPlayer(2f))
        {
            rb2D.MovePosition(Vector2.MoveTowards(transform.position, playerMovePos, moveSpeed * Time.fixedDeltaTime));
        }
        else
        {
            rb2D.MovePosition(Vector2.MoveTowards(transform.position, originMovePos, moveSpeed * Time.fixedDeltaTime));
        }
    }


    IEnumerator UpdateOriginMovePos()
    {
        while (true)
        {
            float x = UnityEngine.Random.Range(0.5f, MaxChangeValue);
            float y = UnityEngine.Random.Range(0.5f, MaxChangeValue);
            x = ConvertPos(x);
            y = ConvertPos(y);

            originMovePos = new Vector3(originPos.x + x, originPos.y + y, originPos.z);

            var sec = UnityEngine.Random.Range(0.1f, 2f);
            yield return new WaitForSeconds(sec);
        }
    }

    IEnumerator UpdatePlayerMovePos()
    {
        while (true)
        {
            float x = UnityEngine.Random.Range(0.2f, 0.5f);
            float y = UnityEngine.Random.Range(0.2f, 0.5f);
            x = ConvertPos(x);
            y = ConvertPos(y);

            var playerPos = gm.player.transform.position;
            playerMovePos = new Vector3(playerPos.x + x, playerPos.y + y, playerPos.z);

            var sec = UnityEngine.Random.Range(0.1f, 2f);
            yield return new WaitForSeconds(sec);
        }
    }

    private float ConvertPos(float pos)
    {
        if (UnityEngine.Random.Range(0, 2) > 0)
        {
            return +pos;
        }
        else
        {
            return -pos;
        }
    }
}
