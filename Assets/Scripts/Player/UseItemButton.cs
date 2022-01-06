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
        ItemInfo equippedItem = PlayerPrefsUtility.Load("equippedItem", new ItemInfo());

        // 何かしらの装備中のアイテムがある
        if (equippedItem._type != e_ItemType.none)
        {
            if (equippedItem._isMultiple)
            {
                // 複数所持可能なアイテムなら一つ消費する
                ReduceOneItem(equippedItem);
            }

            UseItem(equippedItem);
        }
    }

    /// <summary>
    /// アイテムを一つ消費する
    /// </summary>
    private void ReduceOneItem(ItemInfo equippedItem)
    {
        var itemJsonList = PlayerPrefsUtility.LoadList<string>("itemList");
        List<ItemInfo> itemInfoList = itemJsonList.Select(x => JsonUtility.FromJson<ItemInfo>(x)).ToList();
        if (itemInfoList.Any(x => x._type == equippedItem._type))
        {
            itemInfoList.Remove(itemInfoList.First(x => x._type == equippedItem._type));
            PlayerPrefsUtility.SaveJsonList("itemList", itemInfoList);
        }

        // 消費して一つも残らなくなった場合
        if (!itemInfoList.Any(x => x._type == equippedItem._type))
        {
            // UIの装備中アイテムアイコンの設定
            GameManager.Instance.stageUiView.SetItemIconImage(new ItemInfo());

            // 装備中のアイテムをリセット
            PlayerPrefs.DeleteKey("equippedItem");
        }
    }

    private void UseItem(ItemInfo equippedItem)
    {
        Transform itemObj = null;
        switch (equippedItem._type)
        {
            case e_ItemType.magnet:
                itemObj = Instantiate(magnetPrefab);
                break;
            case e_ItemType.bomb:
                itemObj = Instantiate(bombPrefab);
                break;
            case e_ItemType.smallKey:
                itemObj = Instantiate(smallKeyPrefab);
                break;
        }

        if (gm.player.IsOnRight)
        {
            itemObj.position = gm.player.transform.position + new Vector3(0.5f, 0f, 0f);
        }
        else
        {
            itemObj.position = gm.player.transform.position + new Vector3(-0.5f, 0f, 0f);
        }
        
        itemObj.GetComponent<UseItemBase>().Use();
    }
}
