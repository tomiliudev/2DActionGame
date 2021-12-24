using UnityEngine;
using UnityEngine.UI;

public class SlotBase<T> : ButtonBase<ISlotButton>
    where T : IEquipObjectInfo
{
    [SerializeField] Image slotImage;
    protected T slotInfo;
    public void SetSlotInfo(GameObject targetObj, T slotInfo)
    {
        base.targetObj = targetObj;
        this.slotInfo = slotInfo;
        slotImage.sprite = slotInfo.GetSprite();
    }

    public override void Execute(ISlotButton controller)
    {
        controller.OnSlotClicked(slotInfo);
    }
}
