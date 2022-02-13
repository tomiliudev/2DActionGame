public static class GameUtility
{
    public static bool IsGamePause
    {
        get
        {
            var currentGamemode = GameManager.Instance.CurrentGameMode;
            return currentGamemode != e_GameMode.Normal && currentGamemode != e_GameMode.Title;
        }
    }
}
