using System.Linq;
using UnityEngine;

public class EnemyCollisionCheck : MonoBehaviour
{
    [SerializeField] string[] tragetTags;

    private bool isOn;
    public bool IsOn
    {
        get
        {
            return isOn;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (tragetTags.Contains(collision.tag))
        {
            isOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (tragetTags.Contains(collision.tag))
        {
            isOn = false;
        }
    }
}
