using UnityEngine;
using UnityEngine.UI;

public class WeaponSlot : MonoBehaviour
{
    [SerializeField] Image weaponImage;
    private WeaponInfo weaponInfo;

    public void SetWeaponInfo(WeaponInfo weaponInfo)
    {
        this.weaponInfo = weaponInfo;
        weaponImage.sprite = weaponInfo.weaponSprite;
    }

    public void OnClicked()
    {
        Debug.Log(string.Format("{0}がクリックされた！", weaponInfo.weaponType));
    }
}
