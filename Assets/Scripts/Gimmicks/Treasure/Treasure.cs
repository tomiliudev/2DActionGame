using System.Collections;
using UnityEngine;

public sealed class Treasure : MonoBehaviour
{
    [SerializeField] Animator treasureAnimator;
    [SerializeField] Animator speechBubbleAnimator;
    [SerializeField] GameObject objectPrefab;

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

        var obj = Instantiate(objectPrefab, transform.parent, false);
        obj.transform.position = transform.position;
        float yPos = obj.transform.position.y;
        Hashtable hash = new Hashtable();
        hash.Add("y", yPos + 1.5f);
        hash.Add("time", 0.5f);
        iTween.MoveTo(obj, hash);
    }
}
