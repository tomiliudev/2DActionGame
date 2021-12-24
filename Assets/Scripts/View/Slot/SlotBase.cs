using UnityEngine;
using UnityEngine.UI;

public class SlotBase<T> : ButtonBase<IEquipButton>
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

    public override void Execute(IEquipButton controller)
    {
        controller.OnSlotClicked(slotInfo);
    }
}
