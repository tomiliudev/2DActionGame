using UnityEngine;

public class Treasure : MonoBehaviour
{
    public bool IsGetTreasure { get; private set; }

    private void Start()
    {
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            IsGetTreasure = true;
            Destroy(gameObject);
        }
    }
}
