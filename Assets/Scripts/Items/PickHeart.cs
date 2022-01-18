using UnityEngine;

public sealed class PickHeart : MonoBehaviour
{
    [SerializeField] AudioClip pickupSe;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            SoundManager.Instance.Play(pickupSe);

            GameManager.Instance.PlayerCurrentHp++;
            Destroy(gameObject);
        }
    }
}
