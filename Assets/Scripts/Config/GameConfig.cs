public static class GameConfig
{
    public const string PlayerMaxHp = "playerMaxHp";
    public const string WeaponList = "weaponList";
    public const string ItemList = "itemList";
    public const string ItemShopList = "itemShopList";
    public const string EquippedWeapon = "equippedWeapon";
    public const string EquippedItem = "equippedItem";
    public const string TotalPoint = "TotalPoint";

    /// <summary>
    /// 装備中武器を返却する
    /// </summary>
    /// <returns></returns>
    public static WeaponInfo GetEquippedWeapon()
    {
        return PlayerPrefsUtility.Load(EquippedWeapon, new WeaponInfo());
    }

    public static ItemInfo GetEquippedItem()
    {
        return PlayerPrefsUtility.Load(EquippedItem, new ItemInfo());
    }
}