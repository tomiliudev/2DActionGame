using System.Collections;
using UnityEngine;

public abstract class UseItemBase : MonoBehaviour
{
    [SerializeField] protected ItemInfo itemInfo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            float yPos = transform.position.y;
            Hashtable hash = new Hashtable();
            hash.Add("y", yPos + 1.5f);
            hash.Add("time", 0.5f);
            hash.Add("oncomplete", "OnComplete");
            iTween.MoveTo(gameObject, hash);
        }
    }

    private void OnComplete()
    {
        Destroy(gameObject);
    }

    // アイテムを使用する
    public abstract void Use();
}
