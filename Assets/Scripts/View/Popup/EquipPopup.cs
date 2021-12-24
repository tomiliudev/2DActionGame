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
        GenerateSlot<WeaponInfo>("weaponList", weaponSlotList, weaponSlotPrefab);
        GenerateSlot<ItemInfo>("itemList", itemSlotList, itemSlotPrefab);
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
        int slotIdx = 0;
        foreach (Transform slot in slotList.GetComponentsInChildren<Transform>())
        {
            // GetComponentsInChildrenは自分自身も含まれるので、それを除外しないといけない
            if (slot == slotList) continue;
            if (slotIdx >= slotDataList.Count()) break;
            T equipObjectInfo = JsonUtility.FromJson<T>(slotDataList[slotIdx]);
            var slotObj = Instantiate(slotPrefab);
            slotObj.SetSlotInfo(gameObject, equipObjectInfo);
            slotObj.transform.SetParent(slot, false);
            slotObj.gameObject.SetActive(true);
            slotIdx++;
        }
    }

    public void OnSlotClicked(IEquipObjectInfo info)
    {
        if (typeof(WeaponInfo) == info.GetType())
        {
            gm.equippedWeapon = ((WeaponInfo)info).weaponType;
            PlayerPrefsUtility.Save("equippedWeapon", (WeaponInfo)info);
            SetEquippedWeaponImage();
        }
        else if(typeof(ItemInfo) == info.GetType())
        {
            gm.equippedItem = ((ItemInfo)info).itemType;
            PlayerPrefsUtility.Save("equippedItem", (ItemInfo)info);
            SetEquippedItemImage();
        }
    }

    private void SetEquippedWeaponImage()
    {
        var info = PlayerPrefsUtility.Load("equippedWeapon", new WeaponInfo());
        equippedWeaponImage.sprite = info.weaponSprite;
        equippedWeaponImage.gameObject.SetActive(info.weaponType != e_WeaponType.none);
    }

    private void SetEquippedItemImage()
    {
        var info = PlayerPrefsUtility.Load("equippedItem", new ItemInfo());
        equippedItemImage.sprite = info.itemSprite;
        equippedItemImage.gameObject.SetActive(info.itemType != e_ItemType.none);
    }
}
