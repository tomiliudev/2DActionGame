public class ItemUiSlot : UiSlotBase
{
    private void Start()
    {
        base._equipInfo = PlayerPrefsUtility.Load("equippedItem", new ItemInfo());
        base.Start();
    }
}
