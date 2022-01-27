using System;
using System.Collections;
using UnityEngine;

public enum e_ItemType
{
    none,
    magnet,
    bomb,
    smallKey,
    heart,
}

public abstract class ItemBase : MonoBehaviour
{
    [SerializeField] protected ItemInfo itemInfo;
    [SerializeField] protected AudioClip itemPickupSe;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SoundManager.Instance.Play(itemPickupSe);

            // 獲得データを保存する
            PlayerPrefsUtility.AddToJsonList(GameConfig.ItemList, itemInfo, itemInfo.IsMultiple);

            // アイテムを獲得したらショップで購入できるようにリストに追加
            ShopItemListUtility.SaveShopItemList(itemInfo.Type);

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
    public ItemInfoScriptableObject itemInfoData;

    public e_ItemType Type
    {
        get
        {
            return itemInfoData != null ? itemInfoData.type : e_ItemType.none;
        }
    }

    public int Price
    {
        get
        {
            return itemInfoData != null ? itemInfoData.price : 0;
        }
    }

    public bool IsMultiple
    {
        get
        {
            return itemInfoData != null ? itemInfoData.isMultiple : false;
        }
    }
}
