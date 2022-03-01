using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class StageSelectionController : BaseController, IStageSelectionButton
{
    [SerializeField] StageSelectionParts stageSelectionParts;
    [SerializeField] Transform parent;

    StageSelectionParts selectedStageParts = null;
    List<StageSelectionParts> stageSelectionList = new List<StageSelectionParts>();

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

        // クリアしたステージ一覧
        var clearStageDic = PlayerPrefsUtility.LoadDict<string, int>(GameConfig.ClearStageDic);

        var stageNames = Enum.GetValues(typeof(e_StageName));
        e_StageName[] stageNameList = new e_StageName[stageNames.Length];
        stageNames.CopyTo(stageNameList, 0);

        foreach (var (stageName, index) in stageNameList.Select((stageName, index) => (stageName, index)))
        {
            var stageSelectionObj = Instantiate(stageSelectionParts, parent, false);
            stageSelectionObj.StageName = stageName;
            stageSelectionObj.SetThumbnail();

            if (initData != null && initData.selectStage == stageName)
            {
                selectedStageParts = stageSelectionObj;
                stageSelectionObj.IsCanSelect = true;
                stageSelectionObj.SwitchMask(false);
            }
            else
            {
                // 一つ前のステージクリア済みなら選択可能
                bool isCanSelect = false;
                int preIndex = index - 1;

                if (stageName == e_StageName.Stage1)
                {
                    // ステージ１なら問答無用で選択可能
                    isCanSelect = true;
                    stageSelectionObj.SwitchMask(false);
                }
                else if (preIndex >= 0)
                {
                    isCanSelect = clearStageDic.ContainsKey(stageNameList[preIndex].ToString());
                }
                stageSelectionObj.IsCanSelect = isCanSelect;
            }
            stageSelectionObj.SetLockIcon();
            

            stageSelectionList.Add(stageSelectionObj);
        }
    }

    public void OnStageSelectionButtonClick(StageSelectionParts stageSelectionParts)
    {
        foreach (var stageSelection in stageSelectionList)
        {
            stageSelection.SwitchMask(stageSelection != stageSelectionParts);
            if (stageSelection == stageSelectionParts) {
                selectedStageParts = stageSelection;
            }
        }
    }

    // OKボタン
    public void OnOkButtonClicked()
    {
        if (selectedStageParts == null) return;
        gm.LoadToTargetStage(selectedStageParts.StageName);
    }

    // SHOPボタン
    public void OnShopButtonClicked()
    {
        gm.popupView.ShowPopup(e_PopupName.shopPopup);
    }

    // Settingボタン
    public void OnSettingButtonClicked()
    {
        gm.popupView.ShowPopup(e_PopupName.settingPopup);
    }
}
