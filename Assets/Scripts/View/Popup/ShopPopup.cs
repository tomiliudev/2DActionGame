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
    [SerializeField] WeaponSlot weaponSlotPrefab;
    [SerializeField] Image selectShopItemImage;
    [SerializeField] ItemImageScriptableObject itemImageData;
    [SerializeField] WeaponImageScriptableObject weaponImageData;

    int _totalPoint = 0;
    private void Start()
    {
        GenerateItemShopSlot();

        _totalPoint = PlayerPrefsUtility.Load(GameConfig.TotalPoint, 0);
        totalPoint.text = _totalPoint.ToString();
    }

    void GenerateItemShopSlot()
    {
        // 持っているアイテム
        List<string> slotDataList = PlayerPrefsUtility.LoadList<string>(GameConfig.ItemList);
        var groupedSlotList = slotDataList.Select(slotJsonData => JsonUtility.FromJson<ItemInfo>(slotJsonData)).Where(x => x.Type != e_ItemType.none).GroupBy(x => x.Type);

        // ショップに並ぶアイテム/武器
        var itemShopList = PlayerPrefsUtility.LoadList<int>(GameConfig.ItemShopList);
        var weaponShopList = PlayerPrefsUtility.LoadList<int>(GameConfig.WeaponShopList);

        // スロットリスト
        var slotList = itemShopSlotList.GetComponentsInChildren<SlotFrame>();

        int slotIdx = 0;
        foreach (var itemType in itemShopList)
        {
            ItemInfo itemInfo = new ItemInfo();
            if (groupedSlotList.Any(x => x.Key == (e_ItemType)itemType))
            {
                var group = groupedSlotList.First(x => x.Key == (e_ItemType)itemType);
                itemInfo = group.First();
                slotList[slotIdx].SetNumText(group.Count());
            }
            else
            {
                var itemInfoDatas = DataManager.Instance.GetItemInfoDatas();
                itemInfo.itemInfoData = itemInfoDatas.First(x => x.type == (e_ItemType)itemType);
                slotList[slotIdx].SetNumText();
            }

            // Slotの設定
            SetItemSlot(itemInfo, slotList[slotIdx]);
            slotIdx++;
        }

        foreach (var weaponType in weaponShopList)
        {
            WeaponInfo weaponInfo = new WeaponInfo();
            var weaponInfoDatas = DataManager.Instance.GetWeaponInfoDatas();
            weaponInfo.weaponInfoData = weaponInfoDatas.First(x => x.type == (e_WeaponType)weaponType);
            slotList[slotIdx].SetNumText();

            // Slotの設定
            SetWeaponSlot(weaponInfo, slotList[slotIdx]);
            slotIdx++;
        }


        //foreach (SlotFrame slotFrame in itemShopSlotList.GetComponentsInChildren<SlotFrame>())
        //{
        //    if (slotIdx >= itemShopList.Count()) break;
        //    var itemType = itemShopList.ElementAt(slotIdx);

        //    ItemInfo itemInfo = new ItemInfo();
        //    if (groupedSlotList.Any(x => x.Key == (e_ItemType)itemType))
        //    {
        //        var group = groupedSlotList.First(x => x.Key == (e_ItemType)itemType);
        //        itemInfo = group.First();
        //        slotFrame.SetNumText(group.Count());
        //    }
        //    else
        //    {
        //        var itemInfoDatas = DataManager.Instance.GetItemInfoDatas();
        //        itemInfo.itemInfoData = itemInfoDatas.First(x => x.type == (e_ItemType)itemType);
        //        slotFrame.SetNumText();
        //    }

        //    // Slotの設定
        //    SetItemSlot(itemInfo, slotFrame);
        //    slotIdx++;
        //}
    }

    private void SetItemSlot(ItemInfo itemInfo, SlotFrame slotFrame)
    {
        var slotObj = Instantiate(itemSlotPrefab, slotFrame.transform, false);
        slotObj.SetSlotInfo(gameObject, slotFrame, itemInfo, GetItemSprite(itemInfo.Type));
        slotObj.transform.SetAsFirstSibling();
        slotObj.gameObject.SetActive(true);
    }

    private void SetWeaponSlot(WeaponInfo weaponInfo, SlotFrame slotFrame)
    {
        var slotObj = Instantiate(weaponSlotPrefab, slotFrame.transform, false);
        slotObj.SetSlotInfo(gameObject, slotFrame, weaponInfo, GetWeaponSprite(weaponInfo.Type));
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

        if (typeof(ItemInfo) == info.GetType())
        {
            SetShopItem((ItemInfo)info);
        }
        else
        {
            SetShopWeapon((WeaponInfo)info);
        }
    }

    private void SetShopItem(ItemInfo info)
    {
        buyButton.ObjInfo = info;
        SetSelectItemImage(selectShopItemImage, info.Type);
        var itemInfoDatas = DataManager.Instance.GetItemInfoDatas();
        selectItemPrice.text = itemInfoDatas.First(x => x.type == info.Type).price.ToString();
    }

    private void SetShopWeapon(WeaponInfo info)
    {
        buyButton.ObjInfo = info;
        SetSelectWeaponImage(selectShopItemImage, info.Type);
        var weaponInfoDatas = DataManager.Instance.GetWeaponInfoDatas();
        selectItemPrice.text = weaponInfoDatas.First(x => x.type == info.Type).price.ToString();
    }

    Sprite GetItemSprite(e_ItemType type)
    {
        if (type == e_ItemType.none) return null;
        return itemImageData.itemSpriteList[(int)type - 1];
    }

    Sprite GetWeaponSprite(e_WeaponType type)
    {
        if (type == e_WeaponType.none) return null;
        return weaponImageData.weaponSpriteList[(int)type - 1];
    }

    // 選択中のアイテム画像
    void SetSelectItemImage(Image target, e_ItemType itemType)
    {
        target.sprite = GetItemSprite(itemType);
        target.preserveAspect = true;
        target.gameObject.SetActive(itemType != 0);
    }

    void SetSelectWeaponImage(Image target, e_WeaponType weaponType)
    {
        target.sprite = GetWeaponSprite(weaponType);
        target.preserveAspect = true;
        target.gameObject.SetActive(weaponType != 0);
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
            if (typeof(ItemInfo) == info.GetType())
            {
                PlayerPrefsUtility.AddToJsonList(GameConfig.ItemList, info, info.IsMultiple);
            }
            else
            {
                List<string> weaponDataList = PlayerPrefsUtility.LoadList<string>(GameConfig.WeaponList);
                var groupedWeaponDataList = weaponDataList.Select(slotJsonData => JsonUtility.FromJson<WeaponInfo>(slotJsonData)).Where(x => x.Type != 0).GroupBy(x => x.Type);

                var weaponInfo = (WeaponInfo)info;
                if (!info.IsMultiple && groupedWeaponDataList.Any(x => x.Key == weaponInfo.Type))
                {
                    return;
                }
                PlayerPrefsUtility.AddToJsonList(GameConfig.WeaponList, info, info.IsMultiple);
            }

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
