using UnityEngine;

public class Treasure : MonoBehaviour
{
    public bool IsGetTreasure { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            IsGetTreasure = true;
        }
    }
}
