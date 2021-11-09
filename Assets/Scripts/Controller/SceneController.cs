using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] Stage1UiView stage1UiView;
    [SerializeField] PolygonCollider2D cameraArea;
    [SerializeField] Player player;
    [SerializeField] Treasure treasure;
    [SerializeField] GameObject gameOverObj;

    bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Player"), false);
    }

    // Update is called once per frame
    void Update()
    {
        if (treasure.IsGetTreasure)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        if (isGameOver) yield break;
        isGameOver = true;

        gameOverObj.SetActive(true);

        player.DoHpBarAnimation(-player.PlayerHp);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
