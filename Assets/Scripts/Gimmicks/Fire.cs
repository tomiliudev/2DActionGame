using UnityEngine;

public class Fire : MonoBehaviour
{
    GameManager gm;
    private void Start()
    {
        gm = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.PlayerTag)
        {
            gm.player.OnDamage(transform.position);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.PlayerTag)
        {
            gm.player.OnDamage(transform.position);
        }
    }
}