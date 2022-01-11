using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager gm = GameManager.Instance;
            gm.IsGameClear = true;
        }
    }
}
