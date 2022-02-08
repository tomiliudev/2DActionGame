using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UseItemBase : MonoBehaviour
{
    // アイテムを使用する
    public virtual void Use()
    {
        ReduceOneItem();
    }

    /// <summary>
    /// アイテムを一つ消費する
    /// </summary>
    private void ReduceOneItem()
    {
        ItemInfo equippedItem = GameConfig.GetEquippedItem();
        if (!equippedItem.IsMultiple) return;

        var gm = GameManager.Instance;

        // 所持中のアイテムのリスト
        var itemJsonList = PlayerPrefsUtility.LoadList<string>(GameConfig.ItemList);
        List<ItemInfo> itemInfoList = itemJsonList.Select(x => JsonUtility.FromJson<ItemInfo>(x)).ToList();

        // 先に獲得したアイテムを消費する
        if (gm.sceneController.GetItems.Any(x => x.Type == equippedItem.Type))
        {
            gm.sceneController.GetItems.Remove(gm.sceneController.GetItems.First(x => x.Type == equippedItem.Type));
        }
        else if (itemInfoList.Any(x => x.Type == equippedItem.Type))
        {
            // 所持中のアイテムがあれば、消費する
            itemInfoList.Remove(itemInfoList.First(x => x.Type == equippedItem.Type));
            PlayerPrefsUtility.SaveJsonList(GameConfig.ItemList, itemInfoList);
        }

        // 消費して一つも残らなくなった場合
        if (!gm.sceneController.GetItems.Any(x => x.Type == equippedItem.Type)
            && !itemInfoList.Any(x => x.Type == equippedItem.Type))
        {
            // UIの装備中アイテムアイコンの設定
            GameManager.Instance.stageUiView.SetItemIconImage(new ItemInfo());

            // 装備中のアイテムをリセット
            GameConfig.ResetEquippedItem();
        }
    }
}
