using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class Fly : Enemy
{
    private Vector3 movePos;
    private Vector3 originPos;
    private Vector3 originMovePos;
    private Vector3 playerMovePos;

    public bool IsHitTorch { get; private set; }

    Vector3 torchPos = Vector3.zero;

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

        List<Collider2D> hitObjs = base.GetAllHitObjs(2f);
        List<string> hitObjNames = hitObjs.Select(x => x.name).ToList();

        if (hitObjNames.Contains(GameConfig.TorchName) && !IsHitTorch)
        {
            IsHitTorch = true;

            Collider2D torchObj = hitObjs.First(x => x.name == GameConfig.TorchName);
            torchPos = torchObj.transform.position;
            torchPos += new Vector3(0f, torchObj.bounds.size.y / 2, 0f);
        }

        if (IsHitTorch)
        {
            movePos = torchPos;
        }
        else if (hitObjNames.Contains(GameConfig.PlayerName))
        {
            movePos = playerMovePos;
        }
        else
        {
            movePos = originMovePos;
        }

        rb2D.MovePosition(Vector2.MoveTowards(transform.position, movePos, moveSpeed * Time.fixedDeltaTime));
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
