using UnityEngine;

public enum e_SceneName
{
    StageSelection,
    Stage1,
    Stage2,
    Stage3,
    Stage4,
    Stage5
}

public static class GameConfig
{
    public const string ClearStageDic = "ClearStageDic";

    public const string PlayerTag = "Player";
    public const string DoorTag = "Door";
    public const string WallTag = "Wall";
    public const string ladderTag = "Ladder";
    public const string TreasureTag = "Treasure";
    public const string PlatformTag = "Platform";
    public const string SpikeTag = "Spike";
    public const string MiniGameGaugeTag = "MiniGameGauge";
    public const string MiniGameNeedleTag = "MiniGameNeedle";

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

    public static void ResetEquippedItem()
    {
        PlayerPrefs.DeleteKey(EquippedItem);
    }
}
