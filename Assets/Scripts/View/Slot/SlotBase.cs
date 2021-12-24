using UnityEngine;
using UnityEngine.UI;

public class SlotBase<T> : MonoBehaviour
    where T : IEquipObjectInfo
{
    [SerializeField] Image slotImage;
    protected T slotInfo;
    public void SetSlotInfo(T slotInfo)
    {
        this.slotInfo = slotInfo;
        slotImage.sprite = slotInfo.GetSprite();
    }
}
