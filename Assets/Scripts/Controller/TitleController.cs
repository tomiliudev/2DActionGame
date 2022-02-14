using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class TitleController : BaseController
{
    [SerializeField] TorchTrigger torchTrigger;
    [SerializeField] PlayButtonObj playButtonObj;
    [SerializeField] Fly fly;


    GameManager gm;
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
        gm = GameManager.Instance;
        gm.CurrentGameMode = e_GameMode.Title;

    }

    private void Update()
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
            GameUtility.Instance.ShakeScreen(10f, 0.1f, 0.1f);

            // PlayButton移動
            playButtonObj.MoveAnime();

            Invoke("AfterShakeScreen", 10f);
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
    }
}
