using UnityEngine.EventSystems;

public interface IRightButton : IEventSystemHandler
{
    void OnRightButtonDown();
    void OnRightButtonUp();
}