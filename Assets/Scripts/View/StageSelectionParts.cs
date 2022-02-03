using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class StageSelectionParts : ButtonBase<IStageSelectionButton>
{
    [SerializeField] Text stageNameText;
    [SerializeField] GameObject frameImage;

    private e_StageName stageName;
    public e_StageName StageName {
        get { return stageName; }
        set
        {
            stageName = value;
            stageNameText.text = stageName.ToString();
        }
    }

    public bool IsClearStage { get; set; }

    private void Start()
    {
        targetObj = FindObjectOfType<StageSelectionController>().gameObject;
    }

    public override void Click(IStageSelectionButton controller)
    {
        // Stage1またはクリア済みのステージなら選択可能
        if (StageName == e_StageName.Stage1 || IsClearStage)
        {
            controller.OnStageSelectionButtonClick(this);
        }
    }

    public void SwitchFrameImage(bool flag)
    {
        frameImage.SetActive(flag);
    }
}
