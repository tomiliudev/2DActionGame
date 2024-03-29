using UnityEngine;

public class BowButton : MonoBehaviour
{
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
    }

    public void OnBowButtonClick()
    {
        if (gm.IsGameClear || gm.IsGameOver) return;
        var info = PlayerPrefsUtility.Load(GameConfig.EquippedWeapon, new WeaponInfo());
        if (info.Type == e_WeaponType.bow)
        {
            gm.player.BowAttack();
        }
    }
}
