using System;
using System.Collections;
using UnityEngine;

public enum e_ItemType
{
    none,
    magnet,
    bomb,
    smallKey,
}

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField] protected ItemInfo itemInfo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // 獲得データを保存する
            PlayerPrefsUtility.AddToJsonList("itemList", itemInfo, itemInfo._isMultiple);

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
}

[Serializable]
public class ItemInfo : IEquipObjectInfo
{
    public e_ItemType _type;
    public bool _isMultiple;

    public string TypeName()
    {
        return _type.ToString();
    }
}
