using System;
using UnityEngine;
using UnityEngine.UI;

public sealed class StageUiView : MonoBehaviour
{
    [Header("カウントダウン秒")] [SerializeField] Text countDownTime;
    [SerializeField] Text heartText;
    //[SerializeField] Text treasureNum;
    [SerializeField] Text totalPoint;
    [SerializeField] WeaponUiSlot weaponUiSlot;
    [SerializeField] ItemUiSlot itemUiSlot;
    [SerializeField] Image blackMask;
    [SerializeField] GameObject useWeaponButton;
    [SerializeField] GameObject useItemButton;

    GameManager gm;// GameManagerのインスタンス

    float countDownSec = 180f;
    public int CountDownSec
    {
        get { return (int)countDownSec; }
        set { countDownSec = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;

        SetCountDownSec();

        InitPlayerHp();
        totalPoint.text = 0.ToString();

        ShowUseWeaponButton();
        ShowUseItemButton();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.IsInitialized) return;
        OnCountDown();
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
        if (GameUtility.Instance.IsGamePause) return;
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
        heartText.text = GameConfig.GetPlayerHpText();
    }

    public void HpHitAnim()
    {
        heartText.text = GameConfig.GetPlayerHpText();
    }

    public void HpPickAnim()
    {
        heartText.text = GameConfig.GetPlayerHpText();
    }

    public void SetWeaponIconImage(WeaponInfo weaponInfo)
    {
        weaponUiSlot.SetWeaponSprite(weaponInfo.Type);
    }

    public void SetItemIconImage(ItemInfo itemInfo)
    {
        itemUiSlot.SetItemSprite(itemInfo.Type);
    }

    public void UpdateCountDownSecText()
    {
        countDownTime.text = new TimeSpan(0, 0, (int)countDownSec).ToString(@"mm\:ss");
    }

    bool isBlackMaskOn = false;
    public void SwitchOnBlackMask()
    {
        isBlackMaskOn = true;
        gm.CurrentGameMode = e_GameMode.None;
        blackMask.gameObject.SetActive(true);
        iTween.ValueTo(
            gameObject,
            iTween.Hash(
                "time", 1f
                , "from", 0f
                , "to", 1f
                , "onupdate", "OnBlackMaskUpdate"
                , "oncomplete", "OnBlackMaskComplete"
            )
        );
    }

    public void SwitchOffBlackMask()
    {
        isBlackMaskOn = false;
        blackMask.gameObject.SetActive(true);
        GameManager.Instance.CurrentGameMode = e_GameMode.None;
        iTween.ValueTo(
            gameObject,
            iTween.Hash(
                "time", 1f
                , "from", 1f
                , "to", 0f
                , "onupdate", "OnBlackMaskUpdate"
                , "oncomplete", "OnBlackMaskComplete"
            )
        );
    }

    private void OnBlackMaskUpdate(float alpha)
    {
        var c = blackMask.color;
        blackMask.color = new Color(c.r, c.g, c.b, alpha);
    }

    private void OnBlackMaskComplete()
    {
        if (isBlackMaskOn)
        {
            gm.IsGameClear = true;
        }
        else
        {
            blackMask.gameObject.SetActive(false);
            gm.CurrentGameMode = e_GameMode.Normal;
        }
    }

    private void SetCountDownSec()
    {
        int sec = CountDownSec - (int)(gm.CurrentStageLevel * 0.1f * CountDownSec);
        if (sec < 60) sec = 60;
        CountDownSec = sec;
    }

    public void ShowUseWeaponButton()
    {
        var weaponInfo = PlayerPrefsUtility.Load(GameConfig.EquippedWeapon, new WeaponInfo());
        useWeaponButton.SetActive(weaponInfo.Type != e_WeaponType.none);
    }

    public void ShowUseItemButton()
    {
        var itemInfo = PlayerPrefsUtility.Load(GameConfig.EquippedItem, new ItemInfo());
        useItemButton.SetActive(itemInfo.Type != e_ItemType.none);
    }
}
