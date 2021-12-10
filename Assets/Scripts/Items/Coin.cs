using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] GameObject score;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            score.SetActive(true);
            Destroy(gameObject);
        }
    }
}
