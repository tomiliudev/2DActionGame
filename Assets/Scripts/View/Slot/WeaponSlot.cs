using UnityEngine;

public class WeaponSlot : SlotBase<WeaponInfo>
{
    public void OnClicked()
    {
        Debug.Log(string.Format("{0}がクリックされた！", base.slotInfo.weaponType));
    }
}
