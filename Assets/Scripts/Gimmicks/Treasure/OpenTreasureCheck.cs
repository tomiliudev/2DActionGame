using UnityEngine;

public sealed class OpenTreasureCheck : MonoBehaviour
{
    [SerializeField] Animator treasureAnimator;
    [SerializeField] Animator speechBubbleAnimator;
    // 宝箱に触れているか
    public bool IsOnTreasure { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            IsOnTreasure = true;
            speechBubbleAnimator.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            IsOnTreasure = false;
            speechBubbleAnimator.gameObject.SetActive(false);
        }
    }
}
