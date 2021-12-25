public class WeaponFrame : UiSlotBase
{
    private void Start()
    {
        base._equipInfo = PlayerPrefsUtility.Load("equippedWeapon", new WeaponInfo());
        base.Start();
    }
}
