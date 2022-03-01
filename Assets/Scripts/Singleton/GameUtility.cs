using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class GameUtility : SingletonMonoBehaviour<GameUtility>
{
    GameManager gm;
    public bool IsGamePause
    {
        get
        {
            if (gm == null) gm = GameManager.Instance;
            var currentGamemode = GameManager.Instance.CurrentGameMode;
            bool isPause = currentGamemode != e_GameMode.Normal && currentGamemode != e_GameMode.Title
                || gm.popupView.ActivePopupNum > 0;
            return isPause;
        }
    }


    public void ShakeScreen(float time, float magnitudeX, float magnitudeY)
    {
        var mainCamera = Camera.main;
        mainCamera.GetComponent<CinemachineBrain>().enabled = false;
        iTween.ShakePosition(
            mainCamera.gameObject,
            iTween.Hash(
                "time", time,
                "x", magnitudeX,
                "y", magnitudeY,
                "oncompletetarget", gameObject,
                "oncomplete", "OnShakeCameraFinish"
            )
        );
    }

    private void OnShakeCameraFinish()
    {
        var mainCamera = Camera.main;
        mainCamera.GetComponent<CinemachineBrain>().enabled = true;
    }

    public string GetCurrentSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }
}
