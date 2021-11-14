using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] Stage1UiView stage1UiView;
    [SerializeField] PolygonCollider2D cameraArea;
    [SerializeField] Player player;
    [SerializeField] GameObject enemyListObj;
    [SerializeField] Transform treasureList;
    [SerializeField] GameObject gameOverObj;
    [SerializeField] GameObject gameClearObj;

    bool isGameOver = false;
    bool isGameClear = false;
    List<Treasure> treasures = new List<Treasure>();

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"), false);
        treasures = treasureList.GetComponentsInChildren<Treasure>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        if (treasures.All(x => x.IsGetTreasure))
        {
            // 宝箱ゲットしたらゲームクリア
            OnGameClear();
        }

        if (stage1UiView.CountDownSec <= 0)
        {
            // 時間になったらゲームオーバー
            StartCoroutine(OnGameOver());
        }

        if (player.PlayerHp <= 0)
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
        if (isGameClear) yield break;
        if (isGameOver) yield break;
        isGameOver = true;

        gameOverObj.SetActive(true);

        player.DoHpBarAnimation(-player.PlayerHp);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnGameClear()
    {
        if (isGameOver) return;

        if (isGameClear)
        {
            if (Input.touchSupported && Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    // 画面タッチで次のステージへ
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }

            if (Application.isEditor)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    // 画面タッチで次のステージへ
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }

            return;
        }

        isGameClear = true;

        gameClearObj.SetActive(true);

        // プレイヤーと敵にIsGameClearを通知
        player.IsGameClear = true;
        List<Enemy> enemies = enemyListObj.GetComponentsInChildren<Enemy>().ToList();
        foreach (Enemy enemy in enemies)
        {
            enemy.IsGameClear = true;
        }

        stage1UiView.IsGameClear = true;
    }
}
