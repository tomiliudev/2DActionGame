using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField] Animator animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "PlayerFoot")
        {
            animator.SetTrigger("Bounce");
        }
    }
}
