using System;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] bool isCheckPlatform;

    enum GroundTagType {
        Ground,
        Platform
    }

    private bool isInGround;
    public bool IsInGround
    {
        get
        {
            return isInGround;
        }
        private set
        {
            isInGround = value;
        }
    }

    private void FixedUpdate()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckOnGround(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        CheckOnGround(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Enum.IsDefined(typeof(GroundTagType), collision.tag))
        {
            IsInGround = false;
        } 
    }

    private void CheckOnGround(Collider2D collision)
    {
        GroundTagType tagType;
        if(Enum.TryParse(collision.tag, out tagType))
        {
            switch (tagType)
            {
                case GroundTagType.Ground:
                    IsInGround = true;
                    break;
                case GroundTagType.Platform:
                    IsInGround = isCheckPlatform ? true : false;
                    break;
            }
        }
    }
}
