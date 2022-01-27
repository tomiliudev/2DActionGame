using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class UseItemButton : MonoBehaviour
{
    [SerializeField] Transform magnetPrefab;
    [SerializeField] Transform bombPrefab;
    [SerializeField] Transform smallKeyPrefab;

    GameManager gm;
    private void Start()
    {
        gm = GameManager.Instance;
    }

    public void OnUseItemButtonClicked()
    {
        if (gm.IsGameClear || gm.IsGameOver) return;

        ItemInfo equippedItem = GameConfig.GetEquippedItem();
        switch (equippedItem.Type)
        {
            case e_ItemType.magnet:
                UseItem(Instantiate(magnetPrefab), equippedItem);
                break;
            case e_ItemType.bomb:
                UseItem(Instantiate(bombPrefab), equippedItem);
                break;
            case e_ItemType.smallKey:
                if (gm.player.TouchingTreasure != null && !gm.player.TouchingTreasure.IsOpened)
                {
                    UseItem(Instantiate(smallKeyPrefab), equippedItem);
                }
                break;
        }
    }

    /// <summary>
    /// アイテムを一つ消費する
    /// </summary>
    private void ReduceOneItem(ItemInfo equippedItem)
    {
        var itemJsonList = PlayerPrefsUtility.LoadList<string>("itemList");
        List<ItemInfo> itemInfoList = itemJsonList.Select(x => JsonUtility.FromJson<ItemInfo>(x)).ToList();
        if (itemInfoList.Any(x => x.Type == equippedItem.Type))
        {
            itemInfoList.Remove(itemInfoList.First(x => x.Type == equippedItem.Type));
            PlayerPrefsUtility.SaveJsonList("itemList", itemInfoList);
        }

        // 消費して一つも残らなくなった場合
        if (!itemInfoList.Any(x => x.Type == equippedItem.Type))
        {
            // UIの装備中アイテムアイコンの設定
            GameManager.Instance.stageUiView.SetItemIconImage(new ItemInfo());

            // 装備中のアイテムをリセット
            PlayerPrefs.DeleteKey(GameConfig.EquippedItem);
        }
    }

    private void UseItem(Transform itemObj, ItemInfo equippedItem)
    {
        itemObj.GetComponent<UseItemBase>().Use();

        if (equippedItem.IsMultiple)
        {
            // 複数所持可能なアイテムなら一つ消費する
            ReduceOneItem(equippedItem);
        }
    }
}
