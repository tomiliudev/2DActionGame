using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EquipPopup : PopupBase
{
    [SerializeField] Toggle weaponToggle;
    [SerializeField] Toggle itemToggle;
    [SerializeField] ScrollRect weaponScroll;
    [SerializeField] ScrollRect itemScroll;
    [SerializeField] Transform weaponSlotList;
    [SerializeField] Transform itemSlotList;
    [SerializeField] WeaponSlot weaponSlotPrefab;

    private void Start()
    {
        SwitchToggle();

        List<string> weaponList = PlayerPrefsUtility.LoadList<string>("weaponList");
        int slotIdx = 0;
        foreach (Transform slot in weaponSlotList.GetComponentsInChildren<Transform>())
        {
            // GetComponentsInChildrenは自分自身も含まれるので、それを除外しないといけない
            if (slot == weaponSlotList) continue;
            if (slotIdx >= weaponList.Count()) break;
            WeaponInfo weaponInfo = JsonUtility.FromJson<WeaponInfo>(weaponList[slotIdx]);
            WeaponSlot weaponSlot = Instantiate(weaponSlotPrefab);
            weaponSlot.SetWeaponInfo(weaponInfo);
            weaponSlot.transform.SetParent(slot, false);
            weaponSlot.gameObject.SetActive(true);
            slotIdx++;
        }
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


    void aaa<T>(T value, Transform slotList) where T : WeaponBase
    {
        int slotIdx = 0;
        foreach (Transform slot in slotList.GetComponentsInChildren<Transform>())
        {
            // GetComponentsInChildrenは自分自身も含まれるので、それを除外しないといけない
            if (slot == slotList) continue;

            slotIdx++;
        }
    }
}
