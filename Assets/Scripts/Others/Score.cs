using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private void OnEnable()
    {
        transform.parent = null;
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
