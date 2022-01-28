using System;

[Serializable]
public sealed class CountDownTimePenalty : BasePenalty
{
    public override void ExePenalty()
    {
        int countDownSec = GameManager.Instance.stageUiView.CountDownSec - 10;
        GameManager.Instance.stageUiView.CountDownSec = countDownSec;
        GameManager.Instance.stageUiView.UpdateCountDownSecText();
    }
}
