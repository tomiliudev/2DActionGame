using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipPopup : PopupBase, ISlotButton
{
    [SerializeField] Toggle weaponToggle;
    [SerializeField] Toggle itemToggle;
    [SerializeField] ScrollRect weaponScroll;
    [SerializeField] ScrollRect itemScroll;
    [SerializeField] Transform weaponSlotList;
    [SerializeField] Transform itemSlotList;
    [SerializeField] WeaponSlot weaponSlotPrefab;
    [SerializeField] ItemSlot itemSlotPrefab;
    [SerializeField] Image equippedWeaponImage;
    [SerializeField] Image equippedItemImage;

    GameManager gm;

    private void Start()
    {
        gm = GameManager.Instance;
        SwitchToggle();
        SetEquippedWeaponImage();
        SetEquippedItemImage();
        GenerateSlot("weaponList", weaponSlotList, weaponSlotPrefab);
        GenerateSlot("itemList", itemSlotList, itemSlotPrefab);
    }

    public void OnToggleChanged()
    {
        SwitchToggle();
    }

    void SwitchToggle()
    {
        weaponScroll.gameObject.SetActive(weaponToggle.isOn);
        itemScroll.gameObject.SetActive(itemToggle.isOn);
    }

    void GenerateSlot<T>(string slotKey, Transform slotList, SlotBase<T> slotPrefab) where T : IEquipObjectInfo
    {
        List<string> slotDataList = PlayerPrefsUtility.LoadList<string>(slotKey);

        Debug.Log(slotKey);
        foreach (var item in slotDataList)
        {
            Debug.Log(item);
        }
        

        var groupedSlotList = slotDataList.Select(slotJsonData => JsonUtility.FromJson<T>(slotJsonData)).GroupBy(x => x.GetSprite().name);

        int slotIdx = 0;
        foreach (SlotFrame slot in slotList.GetComponentsInChildren<SlotFrame>())
        {
            if (slotIdx >= groupedSlotList.Count()) break;
            var group = groupedSlotList.ElementAt(slotIdx);

            // 所持数
            slot.SetNumText(group.Count());

            // Slotの設定
            var slotObj = Instantiate(slotPrefab);
            slotObj.SetSlotInfo(gameObject, group.First());
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
            gm.stageUiView.SetWeaponIconImage((WeaponInfo)info);
        }
        else if(typeof(ItemInfo) == info.GetType())
        {
            PlayerPrefsUtility.SaveToJson("equippedItem", (ItemInfo)info);
            SetEquippedItemImage();

            // UIの装備中アイテムアイコンの設定
            gm.stageUiView.SetItemIconImage((ItemInfo)info);
        }
    }

    private void SetEquippedWeaponImage()
    {
        var info = PlayerPrefsUtility.Load("equippedWeapon", new WeaponInfo());
        equippedWeaponImage.sprite = info._sprite;
        equippedWeaponImage.preserveAspect = true;
        equippedWeaponImage.gameObject.SetActive(info._type != e_WeaponType.none);
    }

    private void SetEquippedItemImage()
    {
        var info = PlayerPrefsUtility.Load("equippedItem", new ItemInfo());
        equippedItemImage.sprite = info._sprite;
        equippedItemImage.preserveAspect = true;
        equippedItemImage.gameObject.SetActive(info._type != e_ItemType.none);
    }
}
