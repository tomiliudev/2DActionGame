using UnityEngine;

public class BowButton : MonoBehaviour
{
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBowButtonClick()
    {
        if (gm.equippedWeapon == e_WeaponType.bow)
        {
            gm.player.BowAttack();
        }
    }
}
