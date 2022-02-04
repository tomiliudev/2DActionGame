using UnityEngine;
using UnityEngine.UI;

public class SlotBase<T> : ButtonBase<ISlotButton>
    where T : IEquipObjectInfo
{
    [SerializeField] Image slotImage;
    protected T slotInfo;
    private SlotFrame _slotFrame;
    public void SetSlotInfo(GameObject targetObj, SlotFrame slotFrame, T slotInfo, Sprite objSprite)
    {
        base.targetObj = targetObj;
        _slotFrame = slotFrame;
        this.slotInfo = slotInfo;
        SetSlotImage(objSprite);
    }

    public override void Click(ISlotButton controller)
    {
        controller.OnSlotClicked(_slotFrame, slotInfo);
    }

    private void SetSlotImage(Sprite objSprite)
    {
        slotImage.sprite = objSprite;
        slotImage.SetNativeSize();
        slotImage.preserveAspect = true;
    }
}
