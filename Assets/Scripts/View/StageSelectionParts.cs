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

    // 選択可能か
    public bool IsCanSelect { get; set; }

    private void Start()
    {
        targetObj = FindObjectOfType<StageSelectionController>().gameObject;
    }

    public override void Click(IStageSelectionButton controller)
    {
        if (IsCanSelect)
        {
            controller.OnStageSelectionButtonClick(this);
        }
    }

    public void SwitchFrameImage(bool flag)
    {
        frameImage.SetActive(flag);
    }
}
