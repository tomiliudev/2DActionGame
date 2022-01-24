public sealed class DownButton : ButtonBase<IDownButton>
{
    private void Start()
    {
        targetObj = GameManager.Instance.player.gameObject;
    }

    public override void Down(IDownButton controller)
    {
        controller.OnDownButtonDown();
    }

    public override void Up(IDownButton controller)
    {
        controller.OnDownButtonUp();
    }
}
