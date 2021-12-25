using UnityEngine;

public class ItemFrame : UiSlotBase
{
    private void Start()
    {
        base._equipInfo = PlayerPrefsUtility.Load("equippedItem", new ItemInfo());
        Debug.Log("2222222222222");
        base.Start();
    }
}
