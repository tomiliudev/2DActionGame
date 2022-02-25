public sealed class ClearPopupOkButton : ButtonBase<IClearPopupOkButton>
{
    public override void Click(IClearPopupOkButton controller)
    {
        controller.OnClearPopupOkButton();
    }
}
