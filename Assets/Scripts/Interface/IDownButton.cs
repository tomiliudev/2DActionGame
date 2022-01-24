using UnityEngine.EventSystems;

public interface IDownButton : IEventSystemHandler
{
    void OnDownButtonDown();
    void OnDownButtonUp();
}