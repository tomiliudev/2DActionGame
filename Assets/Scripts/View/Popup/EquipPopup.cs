using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipPopup : PopupBase, ISlotButton, IBuyButton
{
    [SerializeField] GameObject equipView;
    [SerializeField] GameObject shopView;
    [SerializeField] Toggle weaponToggle;
    [SerializeField] Toggle itemToggle;
    [SerializeField] Toggle shopToggle;
    [SerializeField] GameObject weaponToggleOnText;
    [SerializeField] GameObject itemToggleOnText;
    [SerializeField] GameObject shopToggleOnText;
    [SerializeField] ScrollRect weaponScroll;
    [SerializeField] ScrollRect itemScroll;
    [SerializeField] ScrollRect shopScroll;
    [SerializeField] Transform weaponSlotList;
    [SerializeField] Transform itemSlotList;
    [SerializeField] Transform itemShopSlotList;
    [SerializeField] Text totalPoint;
    [SerializeField] Text selectItemPrice;
    [SerializeField] BuyButton buyButton;

    [SerializeField] WeaponSlot weaponSlotPrefab;
    [SerializeField] ItemSlot itemSlotPrefab;
    [SerializeField] Image equippedWeaponImage;
    [SerializeField] Image equippedItemImage;
    [SerializeField] Image selectShopItemImage;
    [SerializeField] Sprite[] weaponSprites;
    [SerializeField] Sprite[] itemSprites;

    int _totalPoint = 0;
    private void Start()
    {
        SwitchToggle();
        SetEquippedWeaponImage();
        SetEquippedItemImage();
        GenerateWeaponSlot();
        GenerateItemSlot();
        GenerateItemShopSlot();

        _totalPoint = PlayerPrefsUtility.Load(GameConfig.TotalPoint, 0);
        totalPoint.text = _totalPoint.ToString();
    }

    public void OnWeaponToggleChanged()
    {
        weaponToggleOnText.SetActive(weaponToggle.isOn);
        weaponScroll.gameObject.SetActive(weaponToggle.isOn);
        SwitchView();
    }

    public void OnItemToggleChanged()
    {
        itemToggleOnText.SetActive(itemToggle.isOn);
        itemScroll.gameObject.SetActive(itemToggle.isOn);
        SwitchView();
    }

    public void OnShopToggleChanged()
    {
        shopToggleOnText.SetActive(shopToggle.isOn);
        shopScroll.gameObject.SetActive(shopToggle.isOn);
        SwitchView();
    }

    private void SwitchView()
    {
        shopView.SetActive(shopToggle.isOn);
        equipView.SetActive(!shopView.activeSelf);
    }

    void SwitchToggle()
    {
        weaponToggleOnText.SetActive(weaponToggle.isOn);
        weaponScroll.gameObject.SetActive(weaponToggle.isOn);
        itemToggleOnText.SetActive(itemToggle.isOn);
        itemScroll.gameObject.SetActive(itemToggle.isOn);
        shopToggleOnText.SetActive(shopToggle.isOn);
        SwitchView();
    }

    void GenerateWeaponSlot()
    {
        List<string> slotDataList = PlayerPrefsUtility.LoadList<string>(GameConfig.WeaponList);
        var groupedSlotList = slotDataList.Select(slotJsonData => JsonUtility.FromJson<WeaponInfo>(slotJsonData)).GroupBy(x => x._type);

        int slotIdx = 0;
        foreach (SlotFrame slotFrame in weaponSlotList.GetComponentsInChildren<SlotFrame>())
        {
            if (slotIdx >= groupedSlotList.Count()) break;
            var group = groupedSlotList.ElementAt(slotIdx);

            // 所持数
            slotFrame.SetNumText(group.Count());

            // Slotの設定
            var slotObj = Instantiate(weaponSlotPrefab, slotFrame.transform, false);
            slotObj.SetSlotInfo(gameObject, slotFrame, group.First(), GetWeaponSprite(group.First()._type));
            slotObj.transform.SetAsFirstSibling();
            slotObj.gameObject.SetActive(true);
            slotIdx++;
        }
    }

    void GenerateItemSlot()
    {
        // 所持アイテムのグループ情報
        List<string> slotDataList = PlayerPrefsUtility.LoadList<string>(GameConfig.ItemList);
        var groupedSlotList = slotDataList.Select(slotJsonData => JsonUtility.FromJson<ItemInfo>(slotJsonData)).Where(x => x.Type != e_ItemType.none).GroupBy(x => x.Type);

        // 獲得したアイテムのリスト
        var getItems = base.gm.sceneController.GetItems;

        // 所持アイテムと獲得アイテムを混ぜる
        List<ItemInfo> itemList = new List<ItemInfo>();
        foreach (var itemGroup in groupedSlotList)
        {
            itemList.AddRange(itemGroup.Select(x => x));
        }
        itemList.AddRange(getItems);
        var itemGroupList = itemList.GroupBy(x => x.Type);

        int slotIdx = 0;
        foreach (SlotFrame slotFrame in itemSlotList.GetComponentsInChildren<SlotFrame>())
        {
            if (slotIdx >= itemGroupList.Count()) break;
            var group = itemGroupList.ElementAt(slotIdx);

            var itemInfo = group.First();

            // 所持中アイテムの数　＋　獲得したアイテムの数
            int itemCount = group.Count();

            // 所持数
            slotFrame.SetNumText(itemCount);

            // Slotの設定
            SetItemSlot(itemInfo, slotFrame);

            slotIdx++;
        }
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
            SetItemSlot(itemInfo, slotFrame, isShopItem:true);
            slotIdx++;
        }
    }

    IEnumerable<T> Merge<T>(T[] ary1, T[] ary2)
    {
        foreach (T i in ary1)
            yield return i;
        foreach (T i in ary2)
            yield return i;
    }

    private void SetItemSlot(ItemInfo itemInfo, SlotFrame slotFrame, bool isShopItem = false)
    {
        var slotObj = Instantiate(itemSlotPrefab, slotFrame.transform, false);
        slotObj.SetSlotInfo(gameObject, slotFrame, itemInfo, GetItemSprite(itemInfo.Type), isShopItem);
        slotObj.transform.SetAsFirstSibling();
        slotObj.gameObject.SetActive(true);
    }

    // 選択中のスロット
    SlotFrame selectSlotFrame = null;
    /// <summary>
    /// スロットをクリックした時
    /// </summary>
    /// <param name="info"></param>
    /// <param name="isShopItem"></param>
    public void OnSlotClicked(SlotFrame slotFrame, IEquipObjectInfo info, bool isShopItem)
    {
        selectSlotFrame = slotFrame;
        if (isShopItem)
        {
            SetShopItem((ItemInfo)info);
        }
        else
        {
            SetEquipObj(info);
        }
    }

    private void SetShopItem(ItemInfo info)
    {
        buyButton.ObjInfo = info;
        SetSelectItemImage(selectShopItemImage, info.Type);
        var itemInfoDatas = DataManager.Instance.GetItemInfoDatas();
        selectItemPrice.text = itemInfoDatas.First(x => x.type == info.Type).price.ToString();
    }

    private void SetEquipObj(IEquipObjectInfo info)
    {
        if (typeof(WeaponInfo) == info.GetType())
        {
            PlayerPrefsUtility.SaveToJson(GameConfig.EquippedWeapon, (WeaponInfo)info);
            SetEquippedWeaponImage();

            // UIの装備中武器アイコンの設定
            base.gm.stageUiView.SetWeaponIconImage((WeaponInfo)info);
        }
        else if (typeof(ItemInfo) == info.GetType())
        {
            PlayerPrefsUtility.SaveToJson(GameConfig.EquippedItem, (ItemInfo)info);
            SetEquippedItemImage();

            // UIの装備中アイテムアイコンの設定
            base.gm.stageUiView.SetItemIconImage((ItemInfo)info);
        }
    }

    private void SetEquippedWeaponImage()
    {
        var info = GameConfig.GetEquippedWeapon();
        equippedWeaponImage.sprite = GetWeaponSprite(info._type);
        equippedWeaponImage.preserveAspect = true;
        equippedWeaponImage.gameObject.SetActive(info._type != e_WeaponType.none);
    }

    private void SetEquippedItemImage()
    {
        var info = GameConfig.GetEquippedItem();
        SetSelectItemImage(equippedItemImage, info.Type);
    }

    Sprite GetWeaponSprite(e_WeaponType type)
    {
        if (type == e_WeaponType.none) return null;
        return weaponSprites[(int)type - 1];
    }

    Sprite GetItemSprite(e_ItemType type)
    {
        if (type == e_ItemType.none) return null;
        return itemSprites[(int)type - 1];
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
            GameManager.Instance.stageUiView.UpdateTotalPointView(fromPoint, toPoint);
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
