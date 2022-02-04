using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public sealed class ShopPopup : PopupBase, ISlotButton, IBuyButton
{
    [SerializeField] Transform itemShopSlotList;
    [SerializeField] Text totalPoint;
    [SerializeField] Text selectItemPrice;
    [SerializeField] BuyButton buyButton;

    [SerializeField] ItemSlot itemSlotPrefab;
    [SerializeField] Image selectShopItemImage;
    [SerializeField] ItemImageScriptableObject itemImageData;

    int _totalPoint = 0;
    private void Start()
    {
        GenerateItemShopSlot();

        _totalPoint = PlayerPrefsUtility.Load(GameConfig.TotalPoint, 0);
        totalPoint.text = _totalPoint.ToString();
    }

    void GenerateItemShopSlot()
    {
        List<string> slotDataList = PlayerPrefsUtility.LoadList<string>(GameConfig.ItemList);
        var groupedSlotList = slotDataList.Select(slotJsonData => JsonUtility.FromJson<ItemInfo>(slotJsonData)).Where(x => x.Type != e_ItemType.none).GroupBy(x => x.Type);

        var itemShopList = PlayerPrefsUtility.LoadList<int>(GameConfig.ItemShopList);

        int slotIdx = 0;
        foreach (SlotFrame slotFrame in itemShopSlotList.GetComponentsInChildren<SlotFrame>())
        {
            if (slotIdx >= itemShopList.Count()) break;
            var itemType = itemShopList.ElementAt(slotIdx);

            ItemInfo itemInfo = new ItemInfo();
            if (groupedSlotList.Any(x => x.Key == (e_ItemType)itemType))
            {
                var group = groupedSlotList.First(x => x.Key == (e_ItemType)itemType);
                itemInfo = group.First();
                slotFrame.SetNumText(group.Count());
            }
            else
            {
                var itemInfoDatas = DataManager.Instance.GetItemInfoDatas();
                itemInfo.itemInfoData = itemInfoDatas.First(x => x.type == (e_ItemType)itemType);
                slotFrame.SetNumText();
            }

            // Slotの設定
            SetItemSlot(itemInfo, slotFrame);
            slotIdx++;
        }
    }

    private void SetItemSlot(ItemInfo itemInfo, SlotFrame slotFrame)
    {
        var slotObj = Instantiate(itemSlotPrefab, slotFrame.transform, false);
        slotObj.SetSlotInfo(gameObject, slotFrame, itemInfo, GetItemSprite(itemInfo.Type));
        slotObj.transform.SetAsFirstSibling();
        slotObj.gameObject.SetActive(true);
    }

    // 選択中のスロット
    SlotFrame selectSlotFrame = null;
    /// <summary>
    /// スロットをクリックした時
    /// </summary>
    /// <param name="info"></param>
    public void OnSlotClicked(SlotFrame slotFrame, IEquipObjectInfo info)
    {
        selectSlotFrame = slotFrame;
        SetShopItem((ItemInfo)info);
    }

    private void SetShopItem(ItemInfo info)
    {
        buyButton.ObjInfo = info;
        SetSelectItemImage(selectShopItemImage, info.Type);
        var itemInfoDatas = DataManager.Instance.GetItemInfoDatas();
        selectItemPrice.text = itemInfoDatas.First(x => x.type == info.Type).price.ToString();
    }

    Sprite GetItemSprite(e_ItemType type)
    {
        if (type == e_ItemType.none) return null;
        return itemImageData.itemSpriteList[(int)type - 1];
    }

    // 選択中のアイテム画像
    void SetSelectItemImage(Image target, e_ItemType itemType)
    {
        target.sprite = GetItemSprite(itemType);
        target.preserveAspect = true;
        target.gameObject.SetActive(itemType != e_ItemType.none);
    }

    public void OnBuyButtonClicked(IEquipObjectInfo info)
    {
        if (info == null) return;
        if (_totalPoint >= info.Price)
        {
            buyButton.DoBuySe();

            // 金額の計算
            int fromPoint = _totalPoint;
            int toPoint = _totalPoint - info.Price;
            _totalPoint = toPoint;
            UpdateTotalPointView(fromPoint, toPoint);
            PlayerPrefsUtility.Save(GameConfig.TotalPoint, _totalPoint);

            // 購入したオブジェクトを追加
            PlayerPrefsUtility.AddToJsonList(GameConfig.ItemList, info, info.IsMultiple);
            selectSlotFrame.AddNum(1);
        }
    }

    private void UpdateTotalPointView(int from, int to)
    {
        iTween.ValueTo(
            gameObject,
            iTween.Hash(
                "from", from,
                "to", to,
                "time", 0.5f,
                "onupdate", "OnUpdateTotalPointView"
            )
        );
    }

    private void OnUpdateTotalPointView(int point)
    {
        totalPoint.text = point.ToString();
    }
}
