using System.Collections;
using UnityEngine;

public sealed class Score : MonoBehaviour
{
    private void OnEnable()
    {
        transform.SetParent(null);
        var _canvas = GetComponent<Canvas>();
        _canvas.enabled = true;
        float yPos = _canvas.transform.position.y;

        Hashtable hash = new Hashtable();
        hash.Add("y", yPos + 1.5f);
        hash.Add("time", 0.5f);
        hash.Add("oncomplete", "OnComplete");
        iTween.MoveTo(gameObject, hash);
    }

    private void OnComplete()
    {
        Destroy(gameObject);
    }
}
