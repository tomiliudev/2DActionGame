public sealed class RetryButton : ButtonBase<IRetryButton>
{
    public override void Click(IRetryButton controller)
    {
        controller.OnReTryButtonClicked();
    }
}
