using UnityEngine;

public sealed class WeaponUiSlot : UiSlotBase
{
    [SerializeField] Sprite[] weaponSprites;

    private void Start()
    {
        WeaponInfo info = PlayerPrefsUtility.Load("equippedWeapon", new WeaponInfo());
        SetWeaponSprite(info._type);
    }

    public void SetWeaponSprite(e_WeaponType type)
    {
        if (type == e_WeaponType.none)
        {
            base.iconImage.gameObject.SetActive(false);
        }
        else
        {
            base.iconImage.sprite = weaponSprites[(int)type - 1];
            base.iconImage.gameObject.SetActive(true);
            base.iconImage.preserveAspect = true;
        }
    }
}
