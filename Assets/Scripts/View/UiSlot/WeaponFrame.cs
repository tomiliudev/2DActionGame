public class WeaponFrame : UiSlotBase
{
    public void SetIconImage()
    {
        var info = PlayerPrefsUtility.Load("equippedWeapon", new WeaponInfo());
        base.iconImage.sprite = info.weaponSprite;
        base.iconImage.gameObject.SetActive(info.weaponType != e_WeaponType.none);
    }
}
