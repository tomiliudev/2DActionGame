using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipPopup : PopupBase, ISlotButton
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
    [SerializeField] Sprite[] itemSprites;

    private void Start()
    {
        SwitchToggle();
        SetEquippedWeaponImage();
        SetEquippedItemImage();
        GenerateWeaponSlot();
        GenerateItemSlot();
    }

    public void OnToggleChanged()
    {
        SwitchToggle();
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
        List<string> slotDataList = PlayerPrefsUtility.LoadList<string>("weaponList");
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
            slotObj.GetComponent<Image>().preserveAspect = true;
            slotObj.gameObject.SetActive(true);
            slotIdx++;
        }
    }

    void GenerateItemSlot()
    {
        List<string> slotDataList = PlayerPrefsUtility.LoadList<string>("itemList");
        var groupedSlotList = slotDataList.Select(slotJsonData => JsonUtility.FromJson<ItemInfo>(slotJsonData)).GroupBy(x => x.TypeName());

        int slotIdx = 0;
        foreach (SlotFrame slot in itemSlotList.GetComponentsInChildren<SlotFrame>())
        {
            if (slotIdx >= groupedSlotList.Count()) break;
            var group = groupedSlotList.ElementAt(slotIdx);

            // 所持数
            slot.SetNumText(group.Count());

            // Slotの設定
            var slotObj = Instantiate(itemSlotPrefab);
            slotObj.SetSlotInfo(gameObject, group.First(), GetItemSprite(group.First()._type));
            slotObj.transform.SetParent(slot.transform, false);
            slotObj.transform.SetAsFirstSibling();
            slotObj.GetComponent<Image>().preserveAspect = true;
            slotObj.gameObject.SetActive(true);
            slotIdx++;
        }
    }

    public void OnSlotClicked(IEquipObjectInfo info)
    {
        if (typeof(WeaponInfo) == info.GetType())
        {
            PlayerPrefsUtility.SaveToJson("equippedWeapon", (WeaponInfo)info);
            SetEquippedWeaponImage();

            // UIの装備中武器アイコンの設定
            base.gm.stageUiView.SetWeaponIconImage((WeaponInfo)info);
        }
        else if(typeof(ItemInfo) == info.GetType())
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
        equippedItemImage.sprite = GetItemSprite(info._type);
        equippedItemImage.preserveAspect = true;
        equippedItemImage.gameObject.SetActive(info._type != e_ItemType.none);
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
}
