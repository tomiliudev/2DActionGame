using UnityEngine;

public class EquipItemType : MonoBehaviour
{
    [SerializeField] e_ItemType type;
    public e_ItemType ItemType{
        get { return type; }
    }
}
