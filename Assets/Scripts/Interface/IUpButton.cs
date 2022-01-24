using UnityEngine.EventSystems;

public interface IUpButton : IEventSystemHandler
{
    void OnUpButtonDown();
    void OnUpButtonUp();
}