using UnityEngine;

public class Magnet : MonoBehaviour
{
    GameManager gm;
    private void Start()
    {
        gm = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            gm.equippedItem = e_EquipItemType.magnet;
            Destroy(gameObject);
        }
    }
}
