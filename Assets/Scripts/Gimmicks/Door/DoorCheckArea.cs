using UnityEngine;

public sealed class DoorCheckArea : MonoBehaviour
{
    [SerializeField] Animator doorAnimator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            doorAnimator.SetTrigger("open");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            doorAnimator.SetTrigger("close");
        }
    }
}
