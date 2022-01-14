using UnityEngine;

public sealed class ItemUiSlot : UiSlotBase
{
    [SerializeField] Sprite[] itemSprites;

    private void Start()
    {
        ItemInfo info = PlayerPrefsUtility.Load("equippedItem", new ItemInfo());
        SetItemSprite(info._type);
    }

    public void SetItemSprite(e_ItemType type)
    {
        if (type == e_ItemType.none)
        {
            base.iconImage.gameObject.SetActive(false);
        }
        else
        {
            base.iconImage.sprite = itemSprites[(int)type - 1];
            base.iconImage.gameObject.SetActive(true);
            base.iconImage.preserveAspect = true;
        }
    }
}