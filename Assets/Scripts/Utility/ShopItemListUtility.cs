public class ShopItemListUtility
{
    public static void SaveShopItemList(e_ItemType itemType)
    {
        var itemShopList = PlayerPrefsUtility.LoadList<int>(GameConfig.ItemShopList);
        if (!itemShopList.Contains((int)itemType))
        {
            itemShopList.Add((int)itemType);
            PlayerPrefsUtility.SaveList<int>(GameConfig.ItemShopList, itemShopList);
        }
    }

    public static void SaveShopWeaponList(e_WeaponType weaponType)
    {
        var weaponShopList = PlayerPrefsUtility.LoadList<int>(GameConfig.WeaponShopList);
        if (!weaponShopList.Contains((int)weaponType))
        {
            weaponShopList.Add((int)weaponType);
            PlayerPrefsUtility.SaveList<int>(GameConfig.WeaponShopList, weaponShopList);
        }
    }
}
