using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class TitleController : BaseController
{

    GameManager gm;


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

    // OKボタン
    public void OnOkButtonClicked()
    {
    }
}
