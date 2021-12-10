using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] Animator explosionAnimator;
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

    public void OnExplosion()
    {
        explosionAnimator.gameObject.SetActive(true);
        explosionAnimator.SetTrigger("explosion");
        Destroy(gameObject, 0.15f);
    }
}
