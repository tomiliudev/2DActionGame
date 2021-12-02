using System.Linq;
using UnityEngine;

public class WeakBlock : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Debug.Log("Player Enter!");
        }
    }
}
