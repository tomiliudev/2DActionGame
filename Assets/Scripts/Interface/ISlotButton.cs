using UnityEngine.EventSystems;

public interface ISlotButton : IEventSystemHandler
{
    void OnSlotClicked(SlotFrame slotFrame, IEquipObjectInfo info);
}