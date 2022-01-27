using UnityEngine;

public sealed class BuyButton : ButtonBase<IBuyButton>
{
    [SerializeField] AudioClip buySe;

    public IEquipObjectInfo ObjInfo { get; set; }
    public override void Click(IBuyButton controller)
    {
        controller.OnBuyButtonClicked(ObjInfo);
    }

    public void DoBuySe()
    {
        SoundManager.Instance.Play(buySe);
    }
}
