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

    // Start is called before the first frame update
    void Start()
    {
    }

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

    public void SetItemInfo(ItemInfo info)
    {
        itemInfo = info;
    }

    // アイテムを使用する
    public abstract void Use();
}

[Serializable]
public class ItemInfo : IEquipObjectInfo
{
    public e_ItemType _type;
    public Sprite _sprite = null;
    public bool _isMultiple;

    public Sprite GetSprite()
    {
        return _sprite;
    }
}
