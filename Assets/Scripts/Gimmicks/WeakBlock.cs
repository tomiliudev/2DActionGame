using System.Collections;
using UnityEngine;

public class WeakBlock : MonoBehaviour
{
    [SerializeField] Rigidbody2D rg2d;
    [SerializeField] ParticleSystem ps;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "PlayerFoot":
                StartCoroutine(CrushAnimation());
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.tag)
        {
            case "Ground":
                Destroy(gameObject);
                break;
        }
    }

    IEnumerator CrushAnimation()
    {
        float shackTime = 1.5f;
        gameObject.ShakePosition(new Vector2(0.1f, 0f), shackTime, 0f);
        
        ps.Play();

        yield return new WaitForSeconds(shackTime);

        rg2d.velocity = new Vector2(0f, -6f);
    }
}
