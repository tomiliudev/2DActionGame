public sealed class UpButton : ButtonBase<IUpButton>
{
    private void Start()
    {
        targetObj = GameManager.Instance.player.gameObject;
    }

    public override void Down(IUpButton controller)
    {
        controller.OnUpButtonDown();
    }

    public override void Up(IUpButton controller)
    {
        controller.OnUpButtonUp();
    }
}
