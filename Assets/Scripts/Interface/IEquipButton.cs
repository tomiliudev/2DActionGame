using UnityEngine.EventSystems;

public interface IEquipButton : IEventSystemHandler
{
    void OnSlotClicked(IEquipObjectInfo info);
}