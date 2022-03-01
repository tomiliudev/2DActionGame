using UnityEngine.EventSystems;

public interface IPopupCloseButton : IEventSystemHandler
{
    void OnPopupCloseButton(e_PopupName popupName);
}