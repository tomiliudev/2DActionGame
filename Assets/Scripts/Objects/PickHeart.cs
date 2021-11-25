using UnityEngine;

public class PickHeart : MonoBehaviour
{
    public bool IsGetHeart { get; private set; }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            IsGetHeart = true;
            GameManager.Instance.PlayerCurrentHp++;
            Destroy(gameObject);
        }
    }
}
