using System.Collections;
using UnityEngine;

public class Bow : MonoBehaviour
{
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gm.equippedWeapon = e_EquipWeaponType.bow;

            float yPos = transform.position.y;
            Hashtable hash = new Hashtable();
            hash.Add("y", yPos + 1.5f);
            hash.Add("time", 0.5f);
            hash.Add("oncomplete", "OnComplete");
            iTween.MoveTo(gameObject, hash);
        }
    }

    private void OnComplete()
    {
        Destroy(gameObject);
    }
}
