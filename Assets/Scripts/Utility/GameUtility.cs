using Cinemachine;
using UnityEngine;

public class GameUtility : SingletonMonoBehaviour<GameUtility>
{
    public bool IsGamePause
    {
        get
        {
            var currentGamemode = GameManager.Instance.CurrentGameMode;
            return currentGamemode != e_GameMode.Normal && currentGamemode != e_GameMode.Title;
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
}
