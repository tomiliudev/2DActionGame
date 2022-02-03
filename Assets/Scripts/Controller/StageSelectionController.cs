using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class StageSelectionController : MonoBehaviour, IStageSelectionButton
{
    [SerializeField] StageSelectionParts stageSelectionParts;
    [SerializeField] Transform parent;

    StageSelectionParts selectedStageParts = null;
    List<StageSelectionParts> stageSelectionList = new List<StageSelectionParts>();
    void Start()
    {
        foreach (e_StageName stageName in Enum.GetValues(typeof(e_StageName)))
        {
            var stageSelectionObj = Instantiate(stageSelectionParts, parent, false);
            stageSelectionObj.StageName = stageName;
            stageSelectionList.Add(stageSelectionObj);
        }
    }

    public void OnStageSelectionButtonClick(StageSelectionParts stageSelectionParts)
    {
        foreach (var stageSelection in stageSelectionList)
        {
            stageSelection.SwitchFrameImage(stageSelection == stageSelectionParts);
            if (stageSelection == stageSelectionParts) {
                selectedStageParts = stageSelection;
            }
        }
    }

    public void OnOkButtonClicked()
    {
        if (selectedStageParts == null) return;
        GameManager.Instance.LoadToTargetStage(selectedStageParts.StageName);
    }
}
