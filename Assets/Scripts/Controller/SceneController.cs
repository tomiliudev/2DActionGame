using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"), false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.IsInitialized) return;

        OnGameClear();

        if (gm.stageUiView.CountDownSec <= 0)
        {
            // 時間になったらゲームオーバー
            StartCoroutine(OnGameOver());
        }

        if (gm.PlayerCurrentHp <= 0)
        {
            // プレイヤーHPが0になったらゲームオーバー
            StartCoroutine(OnGameOver());
        }

        if (gm.player.transform.localPosition.y < gm.cameraCollider.points.ElementAt(2).y)
        {
            // 画面の下側より落ちた場合ゲームオーバー
            StartCoroutine(OnGameOver());
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

        yield return new WaitForSeconds(1f);

        GameManager.Instance.LoadSceneTo(SceneManager.GetActiveScene().name);
    }

    private void OnGameClear()
    {
        if (gm.IsGameOver) return;

        if (gm.IsGameClear)
        {
            if (Input.touchSupported && Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    // 画面タッチで次のステージへ
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    GameManager.Instance.LoadToNextStage();
                }
            }

            if (Application.isEditor)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // 画面タッチで次のステージへ
                    //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    GameManager.Instance.LoadToNextStage();
                }
            }

            return;
        }
    }
}
