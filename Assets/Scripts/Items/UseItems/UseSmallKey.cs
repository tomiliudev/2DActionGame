public sealed class UseSmallKey : UseItemBase
{
    public override void Use()
    {
        base.Use();
        var gm = GameManager.Instance;
        if (gm.player.TouchingTreasure != null)
        {
            gm.player.TouchingTreasure.Open();
        }
    }
}