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

    enum GroundTagType {
        Ground,
        Platform,
        WeakBlock,
        Box,
        Mushroom,
        Treasure,
        Bridge
    }

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
        GroundTagType tagType;
        if (Enum.TryParse(collision.tag, out tagType))
        {
            // 上に立つもののGameObjectを設定
            gm.standOnObj = collision.gameObject;

            CheckInGround(tagType);
            ShowDust(tagType, collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GroundTagType tagType;
        if (Enum.TryParse(collision.tag, out tagType))
        {
            // 上に立つもののGameObjectを設定
            gm.standOnObj = collision.gameObject;

            CheckInGround(tagType);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 上に立つものから離れればnullでリセット
        gm.standOnObj = null;

        if (Enum.IsDefined(typeof(GroundTagType), collision.tag))
        {
            IsInGround = false;
            IsInMushroom = false;
        } 
    }

    private void CheckInGround(GroundTagType tagType)
    {
        switch (tagType)
        {
            case GroundTagType.Ground:
            case GroundTagType.WeakBlock:
                IsInGround = true;
                break;
            case GroundTagType.Platform:
            case GroundTagType.Box:
            case GroundTagType.Treasure:
            case GroundTagType.Bridge:
                IsInGround = checkType == e_CheckType.foot ? true : false;
                break;
            case GroundTagType.Mushroom:
                IsInMushroom = checkType == e_CheckType.foot ? true : false;
                break;
        }
    }

    private void ShowDust(GroundTagType tagType, Collider2D collision)
    {
        if (checkType != e_CheckType.foot) return;

        switch (tagType)
        {
            case GroundTagType.Ground:
                if (gm != null && gm.player != null) gm.player.ShowDust();
                break;
            case GroundTagType.WeakBlock:
            case GroundTagType.Platform:
            case GroundTagType.Box:
            case GroundTagType.Mushroom:
            case GroundTagType.Treasure:
            case GroundTagType.Bridge:

                Bounds bounds = collision.bounds;
                float appearYpos = bounds.center.y + bounds.extents.y;
                if (transform.position.y >= appearYpos)
                {
                    if (gm != null && gm.player != null) gm.player.ShowDust();
                }
                break;
        }
    }
}
