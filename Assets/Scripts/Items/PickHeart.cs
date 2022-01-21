using UnityEngine;

public sealed class PickHeart : MonoBehaviour
{
    [SerializeField] AudioClip pickupSe;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SoundManager.Instance.Play(pickupSe);

            GameManager.Instance.PlayerCurrentHp++;
            Destroy(gameObject);
        }
    }
}
