using UnityEngine;

public class Explosion : MonoBehaviour
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
            gm.player.OnDamage();
        }
    }
}
