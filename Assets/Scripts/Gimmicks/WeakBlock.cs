using System.Collections;
using UnityEngine;

public sealed class WeakBlock : MonoBehaviour
{
    [SerializeField] Rigidbody2D rg2d;
    [SerializeField] ParticleSystem ps;

    bool isCrush = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case GameConfig.PlayerFootTag:
                StartCoroutine(CrushAnimation());
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.tag)
        {
            case GameConfig.GroundTag:
                Destroy(gameObject);
                break;
        }
    }

    IEnumerator CrushAnimation()
    {
        if (isCrush) yield break;
        isCrush = true;
        float shackTime = 1.5f;
        iTween.ShakePosition(gameObject, new Vector2(0.1f, 0f), shackTime);
        
        ps.Play();

        yield return new WaitForSeconds(shackTime);

        rg2d.velocity = new Vector2(0f, -6f);
    }
}
