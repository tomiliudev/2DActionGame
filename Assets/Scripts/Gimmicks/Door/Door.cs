using UnityEngine;

public sealed class Door : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.PlayerTag)
        {
            GameManager gm = GameManager.Instance;
            gm.IsGameClear = true;
        }
    }
}
