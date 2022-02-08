using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class ItemUiSlot : UiSlotBase
{
    [SerializeField] Sprite[] itemSprites;

    private void Start()
    {
        // 所持中のアイテムのリスト
        var itemJsonList = PlayerPrefsUtility.LoadList<string>(GameConfig.ItemList);
        List<ItemInfo> itemInfoList = itemJsonList.Select(x => JsonUtility.FromJson<ItemInfo>(x)).ToList();

        // 装備中アイテム
        ItemInfo info = GameConfig.GetEquippedItem();
        if (!itemInfoList.Any(x => x.Type == info.Type))
        {
            SetItemSprite(e_ItemType.none);

            // 装備中のアイテムをリセット
            GameConfig.ResetEquippedItem();
        }
        else
        {
            SetItemSprite(info.Type);
        }
    }

    public void SetItemSprite(e_ItemType type)
    {
        if (type == e_ItemType.none)
        {
            base.iconImage.gameObject.SetActive(false);
        }
        else
        {
            base.iconImage.sprite = itemSprites[(int)type - 1];
            base.iconImage.gameObject.SetActive(true);
            base.iconImage.preserveAspect = true;
        }
    }
}
