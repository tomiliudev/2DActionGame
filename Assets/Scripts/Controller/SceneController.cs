using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneController : MonoBehaviour
{
    GameManager gm;

    [SerializeField] AudioSource bgmAudio;
    [SerializeField] AudioClip gameOverSe;
    [SerializeField] GameObject[] doors;
    bool isDoorApear;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;

        gm.stageUiView.SwitchOffBlackMask();
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"), false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.IsInitialized) return;

        DoorApearEvent();

        GameClear();
        GameOver();
    }

    /// <summary>
    /// 扉が表示するイベント
    /// </summary>
    private void DoorApearEvent()
    {
        if (isDoorApear) return;
        if (gm.treasures.All(x => x.IsOpened))
        {
            isDoorApear = true;

            // 扉が出現する(ランダム)
            //GameObject door = doors.OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
            //door.SetActive(true);

            foreach (var door in doors)
            {
                door.SetActive(true);
            }
        }
    }

    // ゲームオーバー
    private IEnumerator OnGameOver()
    {
        if (gm.IsGameClear) yield break;
        if (gm.IsGameOver) yield break;
        gm.IsGameOver = true;

        int currentHp = gm.PlayerCurrentHp;
        for (int i = 1; i <= currentHp; i++)
        {
            gm.PlayerCurrentHp--;
        }

        bgmAudio.Stop();
        SoundManager.Instance.Play(gameOverSe);
        
        yield return new WaitForSeconds(2f);

        gm.popupView.ShowPopup(e_PopupName.gameOverPopup);
        //GameManager.Instance.LoadSceneTo(SceneManager.GetActiveScene().name);
    }

    private void GameOver()
    {
        /*
         * 時間になったらゲームオーバー
         * プレイヤーHPが0になったらゲームオーバー
         * 画面の下側より落ちた場合ゲームオーバー
         */
        if (gm.stageUiView.CountDownSec <= 0
            || gm.PlayerCurrentHp <= 0
            || gm.player.transform.localPosition.y < gm.cameraCollider.points.ElementAt(2).y
        )
        {
            // 時間になったらゲームオーバー
            StartCoroutine(OnGameOver());
        }
    }

    private void GameClear()
    {
        if (gm.IsGameOver) return;

        if (gm.IsGameClear)
        {
            //if (Input.touchSupported && Input.touchCount > 0)
            //{
            //    if (Input.GetTouch(0).phase == TouchPhase.Began)
            //    {
            //        // 画面タッチで次のステージへ
            //        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //        GameManager.Instance.LoadToNextStage();
            //    }
            //}

            //if (Application.isEditor)
            //{
            //    if (Input.GetMouseButtonDown(0))
            //    {
            //        // 画面タッチで次のステージへ
            //        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //        GameManager.Instance.LoadToNextStage();
            //    }
            //}


            GameManager.Instance.LoadToNextStage();
            return;
        }
    }
}
