using UnityEngine;

public class EquipWeaponType : MonoBehaviour
{
    [SerializeField] e_WeaponType type;
    public e_WeaponType WeaponType
    {
        get { return type; }
    }
}
