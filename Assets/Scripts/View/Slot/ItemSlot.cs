using UnityEngine;

public class ItemSlot : SlotBase<ItemInfo>
{
    public void OnClicked()
    {
        Debug.Log(string.Format("{0}がクリックされた！", base.slotInfo.itemType));
    }
}
