using UnityEngine;

public enum e_EquipWeaponType
{
    none,
    bow
}

public class WeaponFrame : MonoBehaviour
{
    [SerializeField] EquipWeaponType[] weaponIcons;

    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (gm != null)
        {
            foreach (var weaponIcon in weaponIcons)
            {
                weaponIcon.gameObject.SetActive(weaponIcon.WeaponType == gm.equippedWeapon);
            }
        }
    }

    public void OnFrameClicked()
    {
        if (gm == null) return;
        gm.popupView.ShowPopup(e_PopupName.equipPopup);
    }
}
