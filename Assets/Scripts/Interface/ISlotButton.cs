using UnityEngine.EventSystems;

public interface ISlotButton : IEventSystemHandler
{
    void OnSlotClicked(IEquipObjectInfo info);
}