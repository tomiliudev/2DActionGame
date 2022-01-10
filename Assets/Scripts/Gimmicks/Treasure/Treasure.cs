using UnityEngine;

public class Treasure : MonoBehaviour
{
    [SerializeField] Animator treasureAnimator;
    [SerializeField] Animator speechBubbleAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!IsOpened)
            {
                speechBubbleAnimator.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            speechBubbleAnimator.gameObject.SetActive(false);
        }
    }

    public bool IsOpened
    {
        get; private set;
    }

    public void Open()
    {
        if (IsOpened) return;
        IsOpened = true;
        treasureAnimator.SetTrigger("open");
        speechBubbleAnimator.gameObject.SetActive(false);
    }
}
