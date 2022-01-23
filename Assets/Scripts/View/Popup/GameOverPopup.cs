using UnityEngine.SceneManagement;

public sealed class GameOverPopup : PopupBase
{
    public void OnReTryButtonClicked()
    {
        StartCoroutine(base.ClosePopup(
            () => { GameManager.Instance.LoadSceneTo(SceneManager.GetActiveScene().name); }
        ));
    }
}
