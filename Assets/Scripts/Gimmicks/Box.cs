using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Explosion explosionAnimator;
    [SerializeField] GameObject fire;
    [SerializeField] GameObject heart;
    [SerializeField] e_DropType dropType;

    enum e_DropType
    {
        none,
        explosion,
        heart,
        coin,
    }

    private int hp = 3;

    public void OnDamage()
    {
        if (hp > 0)
        {
            hp--;
            animator.SetTrigger("hit");
        }

        if(hp <= 0)
        {
            switch (dropType)
            {
                case e_DropType.explosion:
                    Explosion();
                    break;
                case e_DropType.heart:
                    DropHeart();
                    break;
                case e_DropType.coin:
                    DropCoin();
                    break;
            }

            Destroy(gameObject);
        }
    }

    // 爆発
    private void Explosion()
    {
        explosionAnimator.transform.parent = null;
        explosionAnimator.OnExplosion();
        fire.SetActive(true);
        fire.transform.parent = null;
    }

    // ハート
    private void DropHeart()
    {
        heart.transform.parent = null;
        heart.SetActive(true);
    }

    // コイン
    private void DropCoin()
    {

    }
}
