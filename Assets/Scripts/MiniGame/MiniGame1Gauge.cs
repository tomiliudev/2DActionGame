using UnityEngine;

public sealed class MiniGame1Gauge : MonoBehaviour
{
    public bool IsHit { get; private set; }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.MiniGameNeedleTag)
        {
            IsHit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.MiniGameNeedleTag)
        {
            IsHit = false;
        }
    }
}
