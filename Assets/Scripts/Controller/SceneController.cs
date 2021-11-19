using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] StageUiView stageUiView;
    [SerializeField] PolygonCollider2D cameraArea;
    [SerializeField] Player player;
    [SerializeField] Transform treasureList;

    GameManager gm;

    List<Treasure> treasures = new List<Treasure>();

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"), false);
        treasures = treasureList.GetComponentsInChildren<Treasure>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.IsInitialized) return;

        if (treasures.All(x => x.IsGetTreasure))
        {
            // 宝箱ゲットしたらゲームクリア
            OnGameClear();
        }

        if (stageUiView.CountDownSec <= 0)
        {
            // 時間になったらゲームオーバー
            StartCoroutine(OnGameOver());
        }

        if (gm.PlayerCurrentHp <= 0)
        {
            // プレイヤーHPが0になったらゲームオーバー
            StartCoroutine(OnGameOver());
        }

        if (player.transform.localPosition.y < cameraArea.points.ElementAt(2).y)
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

        gm.IsGameClear = true;
    }
}
