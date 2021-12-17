using UnityEngine;

public enum e_EquipItemType
{
    none,
    magnet
}

public class ItemFrame : MonoBehaviour
{
    [SerializeField] EquipItemType[] itemIcons;

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
            foreach (var itemIcon in itemIcons)
            {
                itemIcon.gameObject.SetActive(itemIcon.ItemType == gm.equippedItem);
            }
        }
    }
}
