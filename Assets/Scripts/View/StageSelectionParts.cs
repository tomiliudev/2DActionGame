using UnityEngine;
using UnityEngine.UI;

public sealed class StageSelectionParts : ButtonBase<IStageSelectionButton>
{
    [SerializeField] Image thumbnail;
    [SerializeField] Text stageNameText;
    [SerializeField] GameObject mask;
    [SerializeField] GameObject lockIcon;

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

    public void SetThumbnail()
    {
        Sprite thumbnailSprite = DataManager.Instance.GetTargetThumbnail(StageName.ToString());
        if (thumbnailSprite == null) return;
        thumbnail.sprite = thumbnailSprite;
    }

    public void SwitchMask(bool flag)
    {
        mask.SetActive(flag);
    }

    public void SetLockIcon()
    {
        lockIcon.SetActive(!IsCanSelect);
    }
}
