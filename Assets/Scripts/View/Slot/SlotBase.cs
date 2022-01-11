using UnityEngine;
using UnityEngine.UI;

public class SlotBase<T> : ButtonBase<ISlotButton>
    where T : IEquipObjectInfo
{
    [SerializeField] Image slotImage;
    protected T slotInfo;
    public void SetSlotInfo(GameObject targetObj, T slotInfo, Sprite objSprite)
    {
        base.targetObj = targetObj;
        this.slotInfo = slotInfo;
        slotImage.sprite = objSprite;
    }

    public override void Execute(ISlotButton controller)
    {
        controller.OnSlotClicked(slotInfo);
    }
}
