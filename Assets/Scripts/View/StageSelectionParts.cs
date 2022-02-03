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

    private void Start()
    {
        targetObj = FindObjectOfType<StageSelectionController>().gameObject;
    }

    public override void Click(IStageSelectionButton controller)
    {
        controller.OnStageSelectionButtonClick(this);
    }

    public void SwitchFrameImage(bool flag)
    {
        frameImage.SetActive(flag);
    }
}
