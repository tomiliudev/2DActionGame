using UnityEngine;

public class EquipItemType : MonoBehaviour
{
    [SerializeField] e_EquipItemType type;
    public e_EquipItemType ItemType{
        get { return type; }
    }
}
