using UnityEngine;
using UnityEngine.UI;

public class SlotBase<T> : ButtonBase<ISlotButton>
    where T : IEquipObjectInfo
{
    [SerializeField] Image slotImage;
    protected T slotInfo;
    private bool isShopItem = false;
    public void SetSlotInfo(GameObject targetObj, T slotInfo, Sprite objSprite, bool isShopItem = false)
    {
        base.targetObj = targetObj;
        this.slotInfo = slotInfo;
        this.isShopItem = isShopItem;
        SetSlotImage(objSprite);
    }

    public override void Click(ISlotButton controller)
    {
        controller.OnSlotClicked(slotInfo, isShopItem);
    }

    private void SetSlotImage(Sprite objSprite)
    {
        slotImage.sprite = objSprite;
        slotImage.SetNativeSize();
        slotImage.preserveAspect = true;
    }
}
