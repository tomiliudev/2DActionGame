using UnityEngine;

public class WeaponFrame : UiSlotBase
{
    private void Start()
    {
        base._equipInfo = PlayerPrefsUtility.Load("equippedWeapon", new WeaponInfo());
        Debug.Log("111111111111");
        base.Start();
    }
}
