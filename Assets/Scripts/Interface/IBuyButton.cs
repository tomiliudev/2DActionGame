using UnityEngine.EventSystems;

public interface IBuyButton : IEventSystemHandler
{
    void OnBuyButtonClicked(IEquipObjectInfo info);
}