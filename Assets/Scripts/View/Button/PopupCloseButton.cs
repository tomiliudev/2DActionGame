public sealed class PopupCloseButton : ButtonBase<IPopupCloseButton>
{
    public override void Click(IPopupCloseButton controller)
    {
        e_PopupName popupName = GetComponentInParent<PopupBase>().PopupName;
        controller.OnPopupCloseButton(popupName);
    }
}
