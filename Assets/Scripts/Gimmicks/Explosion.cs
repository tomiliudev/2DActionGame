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
        if (collision.tag == GameConfig.PlayerTag)
        {
            gm.player.OnDamage(transform.position);
        }

        if (collision.tag == GameConfig.EnemyTag)
        {
            collision.GetComponent<Enemy>().OnDamage();
        }
    }

    public void OnExplosion()
    {
        explosionAnimator.gameObject.SetActive(true);
        explosionAnimator.SetTrigger("explosion");
        Destroy(gameObject, 0.15f);
    }
}
