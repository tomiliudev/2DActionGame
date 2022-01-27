using UnityEngine;

public sealed class BuyButton : ButtonBase<IBuyButton>
{
    public IEquipObjectInfo ObjInfo { get; set; }
    public override void Click(IBuyButton controller)
    {
        controller.OnBuyButtonClicked(ObjInfo);
    }
}
