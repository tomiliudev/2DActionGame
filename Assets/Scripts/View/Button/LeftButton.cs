public class LeftButton : ButtonBase<ILeftButton>
{
    public override void Execute(ILeftButton controller)
    {
        controller.OnLeftButton();
    }
}
