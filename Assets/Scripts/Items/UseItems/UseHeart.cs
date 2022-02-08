public sealed class UseHeart : UseItemBase
{
    public override void Use()
    {
        int maxHp = PlayerPrefsUtility.Load(GameConfig.PlayerMaxHp, 1);
        if (maxHp < GameConfig.MaxHp)
        {
            base.Use();
            PlayerPrefsUtility.Save(GameConfig.PlayerMaxHp, maxHp + 1);
            GameManager.Instance.stageUiView.HpPickAnim();
        }
    }
}