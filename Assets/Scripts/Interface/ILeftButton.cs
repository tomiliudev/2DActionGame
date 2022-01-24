using UnityEngine.EventSystems;

public interface ILeftButton : IEventSystemHandler
{
    void OnLeftButtonDown();
    void OnLeftButtonUp();
}