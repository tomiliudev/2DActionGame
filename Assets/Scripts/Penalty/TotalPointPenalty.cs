using System;

[Serializable]
public sealed class TotalPointPenalty : BasePenalty
{
    public override void ExePenalty()
    {
        int totalPoint = PlayerPrefsUtility.Load(GameConfig.TotalPoint, 0);
        int toPoint = totalPoint - 100;
        PlayerPrefsUtility.Save(GameConfig.TotalPoint, toPoint);
        GameManager.Instance.stageUiView.UpdateTotalPointView(totalPoint, toPoint);
    }
}
