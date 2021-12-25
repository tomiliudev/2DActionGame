public class ItemFrame : UiSlotBase
{
    private void Start()
    {
        base._equipInfo = PlayerPrefsUtility.Load("equippedItem", new ItemInfo());
        base.Start();
    }
}
