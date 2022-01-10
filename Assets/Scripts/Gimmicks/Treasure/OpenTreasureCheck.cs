using UnityEngine;

public sealed class OpenTreasureCheck : MonoBehaviour
{
    [SerializeField] Animator animator;
    // 宝箱に触れているか
    public bool IsOnTreasure { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            IsOnTreasure = true;
            animator.SetTrigger("open");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            IsOnTreasure = false;
        }
    }
}
