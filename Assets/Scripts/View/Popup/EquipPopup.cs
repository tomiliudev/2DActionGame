using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipPopup : PopupBase, ISlotButton
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

    [SerializeField] WeaponSlot weaponSlotPrefab;
    [SerializeField] ItemSlot itemSlotPrefab;
    [SerializeField] Image equippedWeaponImage;
    [SerializeField] Image equippedItemImage;
    [SerializeField] Image selectShopItemImage;
    [SerializeField] Sprite[] weaponSprites;
    [SerializeField] Sprite[] itemSprites;

    private void Start()
    {
        SwitchToggle();
        SetEquippedWeaponImage();
        SetEquippedItemImage();
        GenerateWeaponSlot();
        GenerateItemSlot();
        GenerateItemShopSlot();

        totalPoint.text = PlayerPrefsUtility.Load(GameConfig.TotalPoint, 0).ToString();
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
        var groupedSlotList = slotDataList.Select(slotJsonData => JsonUtility.FromJson<WeaponInfo>(slotJsonData)).GroupBy(x => x.TypeName());

        int slotIdx = 0;
        foreach (SlotFrame slot in weaponSlotList.GetComponentsInChildren<SlotFrame>())
        {
            if (slotIdx >= groupedSlotList.Count()) break;
            var group = groupedSlotList.ElementAt(slotIdx);

            // 所持数
            slot.SetNumText(group.Count());

            // Slotの設定
            var slotObj = Instantiate(weaponSlotPrefab);
            slotObj.SetSlotInfo(gameObject, group.First(), GetWeaponSprite(group.First()._type));
            slotObj.transform.SetParent(slot.transform, false);
            slotObj.transform.SetAsFirstSibling();
            slotObj.gameObject.SetActive(true);
            slotIdx++;
        }
    }

    void GenerateItemSlot()
    {
        List<string> slotDataList = PlayerPrefsUtility.LoadList<string>(GameConfig.ItemList);
        var groupedSlotList = slotDataList.Select(slotJsonData => JsonUtility.FromJson<ItemInfo>(slotJsonData)).GroupBy(x => x.TypeName());

        int slotIdx = 0;
        foreach (SlotFrame slot in itemSlotList.GetComponentsInChildren<SlotFrame>())
        {
            if (slotIdx >= groupedSlotList.Count()) break;
            var group = groupedSlotList.ElementAt(slotIdx);

            // 所持数
            slot.SetNumText(group.Count());

            // Slotの設定
            SetItemSlot(group.First(), slot.transform);

            slotIdx++;
        }
    }

    void GenerateItemShopSlot()
    {
        // ハートは最初からショップで購入できるよう追加しておく
        ShopItemListUtility.SaveShopItemList(e_ItemType.heart);

        List<string> slotDataList = PlayerPrefsUtility.LoadList<string>(GameConfig.ItemList);
        var groupedSlotList = slotDataList.Select(slotJsonData => JsonUtility.FromJson<ItemInfo>(slotJsonData)).GroupBy(x => x._type);

        var itemShopList = PlayerPrefsUtility.LoadList<int>(GameConfig.ItemShopList);

        int slotIdx = 0;
        foreach (SlotFrame slot in itemShopSlotList.GetComponentsInChildren<SlotFrame>())
        {
            if (slotIdx >= itemShopList.Count()) break;
            var itemType = itemShopList.ElementAt(slotIdx);

            ItemInfo itemInfo = new ItemInfo();
            if (groupedSlotList.Any(x => x.Key == (e_ItemType)itemType))
            {
                var group = groupedSlotList.First(x => x.Key == (e_ItemType)itemType);
                itemInfo = group.First();
                slot.SetNumText(group.Count());
            }
            else
            {
                itemInfo._type = (e_ItemType)itemType;
            }

            // Slotの設定
            SetItemSlot(itemInfo, slot.transform, isShopItem:true);
            slotIdx++;
        }
    }

    private void SetItemSlot(ItemInfo itemInfo, Transform parent, bool isShopItem = false)
    {
        var slotObj = Instantiate(itemSlotPrefab, parent, false);
        slotObj.SetSlotInfo(gameObject, itemInfo, GetItemSprite(itemInfo._type), isShopItem);
        slotObj.transform.SetAsFirstSibling();
        slotObj.gameObject.SetActive(true);
    }

    /// <summary>
    /// スロットをクリックした時
    /// </summary>
    /// <param name="info"></param>
    /// <param name="isShopItem"></param>
    public void OnSlotClicked(IEquipObjectInfo info, bool isShopItem)
    {
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
        SetSelectItemImage(selectShopItemImage, info._type);
        selectItemPrice.text = info.price.ToString();
    }

    private void SetEquipObj(IEquipObjectInfo info)
    {
        if (typeof(WeaponInfo) == info.GetType())
        {
            PlayerPrefsUtility.SaveToJson("equippedWeapon", (WeaponInfo)info);
            SetEquippedWeaponImage();

            // UIの装備中武器アイコンの設定
            base.gm.stageUiView.SetWeaponIconImage((WeaponInfo)info);
        }
        else if (typeof(ItemInfo) == info.GetType())
        {
            PlayerPrefsUtility.SaveToJson("equippedItem", (ItemInfo)info);
            SetEquippedItemImage();

            // UIの装備中アイテムアイコンの設定
            base.gm.stageUiView.SetItemIconImage((ItemInfo)info);
        }
    }

    private void SetEquippedWeaponImage()
    {
        var info = PlayerPrefsUtility.Load("equippedWeapon", new WeaponInfo());
        equippedWeaponImage.sprite = GetWeaponSprite(info._type);
        equippedWeaponImage.preserveAspect = true;
        equippedWeaponImage.gameObject.SetActive(info._type != e_WeaponType.none);
    }

    private void SetEquippedItemImage()
    {
        var info = PlayerPrefsUtility.Load("equippedItem", new ItemInfo());
        SetSelectItemImage(equippedItemImage, info._type);
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
}
