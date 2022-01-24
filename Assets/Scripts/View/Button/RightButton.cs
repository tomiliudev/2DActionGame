public sealed class RightButton : ButtonBase<IRightButton>
{
    private void Start()
    {
        targetObj = GameManager.Instance.player.gameObject;
    }

    public override void Down(IRightButton controller)
    {
        controller.OnRightButtonDown();
    }

    public override void Up(IRightButton controller)
    {
        controller.OnRightButtonUp();
    }
}
