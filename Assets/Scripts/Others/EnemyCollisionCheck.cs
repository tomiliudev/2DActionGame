using UnityEngine;

public class EnemyCollisionCheck : MonoBehaviour
{
    private const string GroundTag = "Ground";
    private const string EnemyTag = "Enemy";

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
        if (collision.tag == GroundTag || collision.tag == EnemyTag)
        {
            isOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == GroundTag || collision.tag == EnemyTag)
        {
            isOn = false;
        }
    }
}
