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
        var info = PlayerPrefsUtility.Load("equippedWeapon", new WeaponInfo());
        if (info._type == e_WeaponType.bow)
        {
            gm.player.BowAttack();
        }
    }
}
