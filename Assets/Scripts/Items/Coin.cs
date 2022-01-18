using UnityEngine;

public sealed class Coin : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSe;
    [SerializeField] GameObject score;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SoundManager.Instance.Play(coinPickupSe);

            score.SetActive(true);
            Destroy(gameObject);
        }
    }
}
