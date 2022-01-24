public class LeftButton : ButtonBase<ILeftButton>
{
    private void Start()
    {
        targetObj = GameManager.Instance.player.gameObject;
    }

    public override void Down(ILeftButton controller)
    {
        controller.OnLeftButtonDown();
    }

    public override void Up(ILeftButton controller)
    {
        controller.OnLeftButtonUp();
    }
}
