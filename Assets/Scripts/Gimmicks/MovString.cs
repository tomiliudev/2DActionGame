using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovString : MonoBehaviour
{
    public float Top
    {
        get { return _boundsTopLeftCorner.y; }
    }

    public float Bottom
    {
        get { return _boundsBottomLeftCorner.y; }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetRaycastInfo();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(_boundsTopLeftCorner, Vector2.left * 10f, Color.red);
        Debug.DrawRay(_boundsBottomLeftCorner, Vector2.right * 10f, Color.blue);
    }

    Vector2 _boundsTopLeftCorner;
    Vector2 _boundsTopRightCorner;
    Vector2 _boundsBottomLeftCorner;
    Vector2 _boundsBottomRightCorner;
    private void SetRaycastInfo()
    {
        var boxCo2d = GetComponent<BoxCollider2D>();
        float top = boxCo2d.offset.y + (boxCo2d.size.y / 2f);
        float bottom = boxCo2d.offset.y - (boxCo2d.size.y / 2f);
        float left = boxCo2d.offset.x - (boxCo2d.size.x / 2f);
        float right = boxCo2d.offset.x + (boxCo2d.size.x / 2f);
        _boundsTopLeftCorner.x = left;
        _boundsTopLeftCorner.y = top;
        _boundsTopRightCorner.x = right;
        _boundsTopRightCorner.y = top;
        _boundsBottomLeftCorner.x = left;
        _boundsBottomLeftCorner.y = bottom;
        _boundsBottomRightCorner.x = right;
        _boundsBottomRightCorner.y = bottom;
        _boundsTopLeftCorner = transform.TransformPoint(_boundsTopLeftCorner);
        _boundsTopRightCorner = transform.TransformPoint(_boundsTopRightCorner);
        _boundsBottomLeftCorner = transform.TransformPoint(_boundsBottomLeftCorner);
        _boundsBottomRightCorner = transform.TransformPoint(_boundsBottomRightCorner);
    }
}
