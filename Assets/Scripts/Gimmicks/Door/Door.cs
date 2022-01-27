using UnityEngine;

public sealed class Door : MonoBehaviour
{
    [SerializeField] GameObject doorArrow;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.PlayerTag)
        {
            doorArrow.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.PlayerTag)
        {
            doorArrow.SetActive(false);
        }
    }
}
