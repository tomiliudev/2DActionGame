using UnityEngine.EventSystems;

public interface IStageSelectionButton : IEventSystemHandler
{
    void OnStageSelectionButtonClick(StageSelectionParts stageSelectionParts);
}