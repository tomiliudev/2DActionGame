using UnityEngine.EventSystems;

public interface IRetryButton : IEventSystemHandler
{
    void OnReTryButtonClicked();
}