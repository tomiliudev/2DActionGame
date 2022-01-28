using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public sealed class StageUiView : MonoBehaviour
{
    [Header("カウントダウン秒")] [SerializeField] Text countDownTime;
    [SerializeField] GameObject heartPrefab;
    [SerializeField] Transform heartBar;
    [SerializeField] GameObject gameClearObj;
    //[SerializeField] Text treasureNum;
    [SerializeField] Text totalPoint;
    [SerializeField] WeaponUiSlot weaponUiSlot;
    [SerializeField] ItemUiSlot itemUiSlot;

    GameManager gm;// GameManagerのインスタンス

    float countDownSec = 600f;
    int _currentTotalPoint = 0;
    int _totalPoint = 0;
    public int CountDownSec
    {
        get { return (int)countDownSec; }
        set { countDownSec = value; }
    }

    GameObject[] playerHpPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        InitPlayerHp();
        totalPoint.text = PlayerPrefsUtility.Load(GameConfig.TotalPoint, 0).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.IsInitialized) return;
        OnCountDown();

        SetIsGameClear(gm.IsGameClear);
    }

    public void UpdateTotalPointView(int from, int to)
    {
        iTween.ValueTo(
            gameObject,
            iTween.Hash(
                "from", from,
                "to", to,
                "time", 0.5f,
                "onupdate", "OnUpdateTotalPointView"
            )
        );
    }

    private void OnUpdateTotalPointView(int point)
    {
        totalPoint.text = point.ToString();
    }

    private void OnCountDown()
    {
        if (gm.CurrentGameMode != e_GameMode.Normal) return;
        if (gm.IsGameClear) return;
        if (countDownSec > 0f)
        {
            countDownSec -= Time.deltaTime;
        }
        else
        {
            countDownSec = 0f;
        }
        UpdateCountDownSecText();
    }

    private void InitPlayerHp()
    {
        playerHpPrefabs = new GameObject[gm.PlayerMaxHp];
        for (int i = 0; i < gm.PlayerMaxHp; i++)
        {
            playerHpPrefabs[i] = Instantiate(heartPrefab, heartBar);
        }
    }

    public void HpHitAnim()
    {
        if(playerHpPrefabs != null) playerHpPrefabs[gm.PlayerCurrentHp].GetComponent<Animator>().Play("PlayerHpHit");
    }

    public void HpPickAnim()
    {
        if (playerHpPrefabs != null) playerHpPrefabs[gm.PlayerCurrentHp - 1].GetComponent<Animator>().Play("PlayerHpPick");
    }

    public void SetIsGameClear(bool flag)
    {
        gameClearObj.SetActive(flag);
    }

    public void SetWeaponIconImage(WeaponInfo weaponInfo)
    {
        weaponUiSlot.SetWeaponSprite(weaponInfo._type);
    }

    public void SetItemIconImage(ItemInfo itemInfo)
    {
        itemUiSlot.SetItemSprite(itemInfo.Type);
    }

    public void UpdateCountDownSecText()
    {
        countDownTime.text = new TimeSpan(0, 0, (int)countDownSec).ToString(@"mm\:ss");
    }
}
