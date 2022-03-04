using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public sealed class EquipPopup : PopupBase, ISlotButton
{
    [SerializeField] Toggle weaponToggle;
    [SerializeField] Toggle itemToggle;
    [SerializeField] GameObject weaponToggleOnText;
    [SerializeField] GameObject itemToggleOnText;
    [SerializeField] ScrollRect weaponScroll;
    [SerializeField] ScrollRect itemScroll;
    [SerializeField] Transform weaponSlotList;
    [SerializeField] Transform itemSlotList;

    [SerializeField] WeaponSlot weaponSlotPrefab;
    [SerializeField] ItemSlot itemSlotPrefab;
    [SerializeField] Image equippedWeaponImage;
    [SerializeField] Image equippedItemImage;
    [SerializeField] Sprite[] weaponSprites;
    [SerializeField] ItemImageScriptableObject itemImageData;

    private void Start()
    {
        SwitchToggle();
        SetEquippedWeaponImage();
        SetEquippedItemImage();
        GenerateWeaponSlot();
        GenerateItemSlot();
    }

    public void OnWeaponToggleChanged()
    {
        weaponToggleOnText.SetActive(weaponToggle.isOn);
        weaponScroll.gameObject.SetActive(weaponToggle.isOn);
    }

    public void OnItemToggleChanged()
    {
        itemToggleOnText.SetActive(itemToggle.isOn);
        itemScroll.gameObject.SetActive(itemToggle.isOn);
    }

    void SwitchToggle()
    {
        weaponToggleOnText.SetActive(weaponToggle.isOn);
        weaponScroll.gameObject.SetActive(weaponToggle.isOn);
        itemToggleOnText.SetActive(itemToggle.isOn);
        itemScroll.gameObject.SetActive(itemToggle.isOn);
    }

    void GenerateWeaponSlot()
    {
        List<string> slotDataList = PlayerPrefsUtility.LoadList<string>(GameConfig.WeaponList);
        var groupedSlotList = slotDataList.Select(slotJsonData => JsonUtility.FromJson<WeaponInfo>(slotJsonData)).GroupBy(x => x.Type);

        int slotIdx = 0;
        foreach (SlotFrame slotFrame in weaponSlotList.GetComponentsInChildren<SlotFrame>())
        {
            if (slotIdx >= groupedSlotList.Count()) break;
            var group = groupedSlotList.ElementAt(slotIdx);

            // 所持数
            slotFrame.SetNumText(group.Count());

            // Slotの設定
            var slotObj = Instantiate(weaponSlotPrefab, slotFrame.transform, false);
            slotObj.SetSlotInfo(gameObject, slotFrame, group.First(), GetWeaponSprite(group.First().Type));
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
        SetEquipObj(info);
    }

    private void SetEquipObj(IEquipObjectInfo info)
    {
        if (typeof(WeaponInfo) == info.GetType())
        {
            PlayerPrefsUtility.SaveToJson(GameConfig.EquippedWeapon, (WeaponInfo)info);
            SetEquippedWeaponImage();
            gm.stageUiView.ShowUseWeaponButton();

            // UIの装備中武器アイコンの設定
            base.gm.stageUiView.SetWeaponIconImage((WeaponInfo)info);
        }
        else if (typeof(ItemInfo) == info.GetType())
        {
            PlayerPrefsUtility.SaveToJson(GameConfig.EquippedItem, (ItemInfo)info);
            SetEquippedItemImage();
            gm.stageUiView.ShowUseItemButton();

            // UIの装備中アイテムアイコンの設定
            base.gm.stageUiView.SetItemIconImage((ItemInfo)info);
        }
    }

    private void SetEquippedWeaponImage()
    {
        var info = GameConfig.GetEquippedWeapon();
        equippedWeaponImage.sprite = GetWeaponSprite(info.Type);
        equippedWeaponImage.preserveAspect = true;
        equippedWeaponImage.gameObject.SetActive(info.Type != e_WeaponType.none);
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
        return itemImageData.itemSpriteList[(int)type - 1];
    }

    // 選択中のアイテム画像
    void SetSelectItemImage(Image target, e_ItemType itemType)
    {
        target.sprite = GetItemSprite(itemType);
        target.preserveAspect = true;
        target.gameObject.SetActive(itemType != e_ItemType.none);
    }
}
