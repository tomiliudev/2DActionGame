using UnityEngine;

public sealed class UseItemButton : MonoBehaviour
{
    [SerializeField] UseItemBase magnetPrefab;
    [SerializeField] UseItemBase bombPrefab;
    [SerializeField] UseItemBase smallKeyPrefab;
    [SerializeField] UseItemBase heartPrefab;

    GameManager gm;
    private void Start()
    {
        gm = GameManager.Instance;
    }

    public void OnUseItemButtonClicked()
    {
        if (gm.IsGameClear || gm.IsGameOver) return;

        ItemInfo equippedItem = GameConfig.GetEquippedItem();
        switch (equippedItem.Type)
        {
            case e_ItemType.magnet:
                magnetPrefab.Use();
                break;
            case e_ItemType.bomb:
                // 実態化して使う
                var bomb = Instantiate(bombPrefab);
                bomb.Use();
                break;
            case e_ItemType.smallKey:
                if (gm.player.TouchingTreasure != null && !gm.player.TouchingTreasure.IsOpened)
                {
                    smallKeyPrefab.Use();
                }
                break;
            case e_ItemType.heart:
                heartPrefab.Use();
                break;
        }
    }
}
