public class ItemFrame : UiSlotBase
{
    public void SetIconImage()
    {
        var info = PlayerPrefsUtility.Load("equippedItem", new ItemInfo());
        base.iconImage.sprite = info.itemSprite;
        base.iconImage.gameObject.SetActive(info.itemType != e_ItemType.none);
    }
}
