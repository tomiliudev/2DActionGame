public sealed class UseSmallKey : UseItemBase
{
    public override void Use()
    {
        if (base.gm.player.TouchingTreasure != null)
        {
            base.RiseAnimation();
            base.gm.player.TouchingTreasure.Open();
        }
    }
}