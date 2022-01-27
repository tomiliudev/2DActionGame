public static class ShopItemListUtility
{
    public static void SaveShopItemList(e_ItemType itemType)
    {
        var itemShopList = PlayerPrefsUtility.LoadList<int>(GameConfig.ItemShopList);
        if (!itemShopList.Contains((int)itemType))
        {
            itemShopList.Add((int)itemType);
        }
        PlayerPrefsUtility.SaveList<int>(GameConfig.ItemShopList, itemShopList);
    }
}
