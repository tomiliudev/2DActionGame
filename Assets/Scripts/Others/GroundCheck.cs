using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    enum e_CheckType
    {
        head,
        foot
    }
    [SerializeField] e_CheckType checkType = e_CheckType.foot;

    GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance;
    }

    private bool isInGround;
    public bool IsInGround { get { return isInGround; } private set { isInGround = value; } }

    private bool isInMushroom;
    public bool IsInMushroom { get { return isInMushroom; } private set { isInMushroom = value; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckInGround(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckInGround(collision, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 上に立つものから離れればnullでリセット
        gm.standOnObj = null;

        IsInGround = false;
        IsInMushroom = false;
    }

    private Vector2 GetTopLeftPoint(BoxCollider2D col2d)
    {
        float offsetX = col2d.offset.x;
        float offsetY = col2d.offset.y;
        float width = col2d.size.x;
        float height = col2d.size.y;
        float top = offsetY + height / 2;
        float left = offsetX - width / 2;
        Vector2 topLeftCorner = new Vector2(left, top);
        topLeftCorner = col2d.transform.TransformPoint(topLeftCorner);
        return topLeftCorner;
    }

    private void CheckInGround(Collider2D collision, bool isStay = false)
    {
        // 上に立つもののGameObjectを設定
        gm.standOnObj = collision.gameObject;

        switch (collision.tag)
        {
            case GameConfig.GroundTag:
                IsInGround = true;
                if (!isStay) ShowDust();
                break;
            case GameConfig.WeakBlockTag:
            case GameConfig.PlatformTag:
            case GameConfig.TreasureTag:
            case GameConfig.BridgeTag:
            case GameConfig.BoxTag:
                if (checkType == e_CheckType.foot)
                {
                    BoxCollider2D col2d = collision.GetComponent<BoxCollider2D>();
                    if (col2d != null)
                    {
                        Vector2 topLeftCorner = GetTopLeftPoint(col2d);
                        if (Mathf.Abs(transform.position.y - topLeftCorner.y) < 0.18f)
                        {
                            if (!isStay) ShowDust();
                            IsInGround = true;
                        }
                    }
                }
                else
                {
                    IsInGround = false;
                }
                break;
            case GameConfig.MushroomTag:
                IsInMushroom = checkType == e_CheckType.foot ? true : false;
                break;
            
        }
    }

    private void ShowDust()
    {
        if (checkType != e_CheckType.foot) return;
        if (gm != null && gm.player != null) gm.player.ShowDust();
    }

    //private void ShowDust(Collider2D collision)
    //{
    //    if (checkType != e_CheckType.foot) return;

    //    switch (collision.tag)
    //    {
    //        case GameConfig.GroundTag:
    //            if (gm != null && gm.player != null) gm.player.ShowDust();
    //            break;
    //        case GameConfig.WeakBlockTag:
    //        case GameConfig.PlatformTag:
    //        case GameConfig.BoxTag:
    //        case GameConfig.MushroomTag:
    //        case GameConfig.TreasureTag:
    //        case GameConfig.BridgeTag:
    //            Bounds bounds = collision.bounds;
    //            float appearYpos = bounds.center.y + bounds.extents.y;
    //            if (transform.position.y >= appearYpos)
    //            {
    //                if (gm != null && gm.player != null) gm.player.ShowDust();
    //            }
    //            break;
    //    }
    //}
}
