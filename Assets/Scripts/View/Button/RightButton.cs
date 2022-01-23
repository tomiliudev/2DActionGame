public sealed class RightButton : ButtonBase<IRightButton>
{
    public override void Execute(IRightButton controller)
    {
        controller.OnRightButton();
    }
}
