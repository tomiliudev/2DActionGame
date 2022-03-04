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

    bool isGetItem = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.PlayerTag)
        {
            if (isGetItem) return;
            isGetItem = true;

            SoundManager.Instance.Play(itemPickupSe);

            var gm = GameManager.Instance;

            // 獲得したアイテムを追加
            gm.sceneController.GetItems.Add(itemInfo);

            // アイテムを獲得したらショップで購入できるようにリストに追加
            ShopItemListUtility.SaveShopItemList(itemInfo.Type);

            float yPos = transform.position.y;
            Hashtable hash = new Hashtable();
            hash.Add("y", yPos + 1.5f);
            hash.Add("time", 0.5f);
            hash.Add("oncomplete", "OnComplete");
            iTween.MoveTo(gameObject, hash);

            if (GameConfig.GetEquippedItem().Type == e_ItemType.none)
            {
                PlayerPrefsUtility.SaveToJson(GameConfig.EquippedItem, itemInfo);
                gm.stageUiView.SetItemIconImage(itemInfo);
                gm.stageUiView.ShowUseItemButton();
            }
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
