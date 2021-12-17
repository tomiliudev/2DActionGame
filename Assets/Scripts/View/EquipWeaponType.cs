using UnityEngine;

public class EquipWeaponType : MonoBehaviour
{
    [SerializeField] e_EquipWeaponType type;
    public e_EquipWeaponType WeaponType
    {
        get { return type; }
    }
}
