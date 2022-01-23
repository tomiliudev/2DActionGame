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
    [SerializeField] Text treasureNum;
    [SerializeField] WeaponUiSlot weaponUiSlot;
    [SerializeField] ItemUiSlot itemUiSlot;

    GameManager gm;// GameManagerのインスタンス

    float countDownSec = 600f;
    public int CountDownSec
    {
        get { return (int)countDownSec; }
    }

    GameObject[] playerHpPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        InitPlayerHp();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.IsInitialized) return;
        OnCountDown();

        SetIsGameClear(gm.IsGameClear);

        treasureNum.text = string.Format("{0}/{1}", gm.treasures.Count(x => x.IsOpened), gm.treasures.Length);
    }

    private void OnCountDown()
    {
        if (gm.IsGameClear) return;
        if (countDownSec > 0f)
        {
            countDownSec -= Time.deltaTime;
        }
        else
        {
            countDownSec = 0f;
        }
        countDownTime.text = new TimeSpan(0, 0, (int)countDownSec).ToString(@"mm\:ss");
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
        itemUiSlot.SetItemSprite(itemInfo._type);
    }
}
