using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneController : BaseController
{
    GameManager gm;

    [SerializeField] AudioSource bgmAudio;
    [SerializeField] AudioClip gameOverSe;
    [SerializeField] GameObject[] doors;
    bool isDoorApear;

    public int GetPoints { get; set; }

    private List<ItemInfo> getItems = new List<ItemInfo>();
    public List<ItemInfo> GetItems { get { return getItems; } set { getItems = value; } }

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

    bool isDoGameClear = false;
    private void GameClear()
    {
        if (gm.IsGameOver) return;

        if (gm.IsGameClear && !isDoGameClear)
        {
            isDoGameClear = true;

            // クリアしたステージを保存する
            SaveClearedStage();


            // クリアしたら獲得したポイントを保存する
            SavePoints();

            // 獲得データを保存する
            SaveItems();

            // ステージクリアポップアップ
            gm.popupView.ShowPopup(e_PopupName.stageClearPopup);
        }
    }

    // クリアしたステージを保存する
    private void SaveClearedStage()
    {
        // クリアしたステージを記録する
        string clearStageName = SceneManager.GetActiveScene().name;
        var clearStageDic = PlayerPrefsUtility.LoadDict<string, int>(GameConfig.ClearStageDic);
        if (!clearStageDic.ContainsKey(clearStageName))
        {
            clearStageDic.Add(clearStageName, 1);
        }
        else
        {
            int clearLevel = clearStageDic[clearStageName];
            clearStageDic[clearStageName] = clearLevel + 1;
        }
        PlayerPrefsUtility.SaveDict(GameConfig.ClearStageDic, clearStageDic);
    }

    private void SavePoints()
    {
        int totalPoint = PlayerPrefsUtility.Load(GameConfig.TotalPoint, 0);
        PlayerPrefsUtility.Save(GameConfig.TotalPoint, totalPoint + GetPoints);
    }

    private void SaveItems()
    {
        foreach (var itemInfo in GetItems)
        {
            PlayerPrefsUtility.AddToJsonList(GameConfig.ItemList, itemInfo, itemInfo.IsMultiple);
        }
    }
}
