using System.Collections.Generic;
using UnityEngine;

public sealed class DynamicSpike2 : MonoBehaviour
{
    enum Direction
    {
        up,
        down,
        left,
        right
    }
    [SerializeField] Direction direction;
    [SerializeField] BoxCollider2D headCheck;
    [SerializeField] BoxCollider2D headCheck1;
    [SerializeField] BoxCollider2D headCheck2;

    bool isFired = false;
    int PlayerLayerMask;
    int GroundLayerMask;

    Vector2 headPos;
    Vector2 headPos1;
    Vector2 headPos2;
    List<Vector2> headPosList = new List<Vector2>();

    Vector2 dir = Vector2.zero;
    Vector2 targetMovePos = Vector2.zero;

    private void Start()
    {
        PlayerLayerMask = 1 << LayerMask.NameToLayer("Player");
        GroundLayerMask = 1 << LayerMask.NameToLayer("Ground");

        headPos = headCheck.transform.position;
        headPos1 = headCheck1.transform.position;
        headPos2 = headCheck2.transform.position;
        headPosList.Add(headPos);
        headPosList.Add(headPos1);
        headPosList.Add(headPos2);

        switch (direction)
        {
            case Direction.up:
                dir = Vector2.up;
                break;
            case Direction.down:
                dir = Vector2.down;
                break;
            case Direction.left:
                dir = Vector2.left;
                break;
            case Direction.right:
                dir = Vector2.right;
                break;
        }
    }

    private void Update()
    {
        Fire();
    }

    private RaycastHit2D GetHitGround(Vector2 origin, LayerMask layer)
    {
        return Physics2D.Raycast(origin, dir, Mathf.Infinity, layer);
    }

    float targetDistance = 0f;
    private void Fire()
    {
        if (isFired) return;

        Debug.DrawRay(headPos1, dir * 10f, Color.blue);
        Debug.DrawRay(headPos, dir * 10f, Color.red);
        Debug.DrawRay(headPos2, dir * 10f, Color.blue);

        foreach (var _headPos in headPosList)
        {
            var hitPlayer = GetHitGround(_headPos, PlayerLayerMask);
            if (hitPlayer.collider != null)
            {
                var hitGround = GetHitGround(headPos, GroundLayerMask);
                targetMovePos = hitGround.point;
                targetDistance = Vector2.Distance(headPos, targetMovePos);

                FireAnime();
                break;
            }
        }

        //var hitPlayer = GetHitGround(headPos, PlayerLayerMask);
        //if (hitPlayer.collider != null)
        //{
        //    var hitGround = GetHitGround(headPos, GroundLayerMask);
        //    targetMovePos = hitGround.point;
        //    targetDistance = Vector2.Distance(headPos, targetMovePos);

        //    FireAnime();
        //}
    }

    private void FireAnime()
    {
        if (isFired) return;
        isFired = true;
        iTween.MoveTo(gameObject,
            iTween.Hash(
                "x", targetMovePos.x,
                "y", targetMovePos.y,
                "time", 0.5f,
                "easeType", iTween.EaseType.easeOutCubic,
                "oncomplete", "OnUpAnimeComplete"
            )
        );
    }
}