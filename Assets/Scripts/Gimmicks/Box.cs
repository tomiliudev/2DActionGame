using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] Animator explosionAnimator;
    [SerializeField] GameObject fire;
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
            explosionAnimator.transform.parent = null;
            explosionAnimator.SetTrigger("explosion");
            fire.SetActive(true);
            fire.transform.parent = null;
            Destroy(gameObject);
        }
    }
}
