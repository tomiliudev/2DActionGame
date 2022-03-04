using UnityEngine;

public sealed class TitleController : BaseController
{
    [SerializeField] TorchTrigger torchTrigger;
    [SerializeField] PlayButtonObj playButtonObj;
    [SerializeField] Fly fly;

    int isTitleDoorOpened = 0;
    bool isPlayButtonObjOpen = false;
    bool isCutinAnimeDone = false;

    public sealed class InitData : BaseInitData
    {
        public e_StageName selectStage;
        public InitData(e_StageName selectStage)
        {
            this.selectStage = selectStage;
        }
    }

    InitData initData;
    public override void Initialize(BaseInitData initBaseData)
    {
        initData = initBaseData as InitData;
    }

    void Start()
    {
        base.Start();
        gm.CurrentGameMode = e_GameMode.Title;
        isTitleDoorOpened = PlayerPrefsUtility.Load(GameConfig.IsTitleDoorOpened, 0);
    }

    private void Update()
    {
        if (isTitleDoorOpened == 0)
        {
            OpenDoorAnime();
        }
        else
        {
            OpenDoorWithoutAnime();
        }
    }

    private void AfterShakeScreen()
    {
        gm.CurrentGameMode = e_GameMode.Title;
        gm.player.WakeUp();
    }

    private void DoCutinAnime()
    {
        gm.player.Sleep();
        gm.CurrentGameMode = e_GameMode.CutinAnimation;
        PlayerPrefsUtility.Save(GameConfig.IsTitleDoorOpened, 1);
    }

    // ドア開く演出
    private void OpenDoorAnime()
    {
        if (!isCutinAnimeDone && fly.IsHitTorch)
        {
            isCutinAnimeDone = true;
            DoCutinAnime();
        }

        if (!isPlayButtonObjOpen && torchTrigger.IsFlyOn)
        {
            isPlayButtonObjOpen = true;

            // 画面を揺らす
            GameUtility.Instance.ShakeScreen(5f, 0.1f, 0.1f);

            // PlayButton移動
            playButtonObj.MoveAnime();

            Invoke("AfterShakeScreen", 5f);
        }
    }

    private void OpenDoorWithoutAnime()
    {
        fly.gameObject.SetActive(false);
        playButtonObj.transform.position = new Vector3(3f, -3.1f, 0f);
        torchTrigger.FireTorch(true);
    }
}
