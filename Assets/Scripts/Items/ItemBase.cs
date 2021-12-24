using System;
using System.Collections;
using UnityEngine;

public enum e_ItemType
{
    none,
    magnet
}

public abstract class ItemBase : MonoBehaviour
{
    GameManager gm;

    [SerializeField] ItemInfo itemInfo;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // 獲得データを保存する
            PlayerPrefsUtility.SaveJsonList("itemList", itemInfo);
            gm.equippedItem = itemInfo.itemType;

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
    public e_ItemType itemType;
    public Sprite itemSprite;

    public Sprite GetSprite()
    {
        return itemSprite;
    }
}
