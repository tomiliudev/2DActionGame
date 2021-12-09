using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Animator explosionAnimator;
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
            explosionAnimator.gameObject.SetActive(true);
            explosionAnimator.SetTrigger("explosion");
            explosionAnimator.Update(0);
            Destroy(gameObject, explosionAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
        }
    }
}
