using System;

[Serializable]
public sealed class TotalPointPenalty : BasePenalty
{
    public override void ExePenalty()
    {
        int getPoints = GameManager.Instance.sceneController.GetPoints;
        int toPoint = getPoints - 100;
        if (toPoint <= 0) toPoint = 0;
        GameManager.Instance.sceneController.GetPoints = toPoint;
        GameManager.Instance.stageUiView.UpdateTotalPointView(getPoints, toPoint);
    }
}