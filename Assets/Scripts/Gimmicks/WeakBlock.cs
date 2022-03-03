using System.Collections;
using UnityEngine;

public sealed class WeakBlock : MonoBehaviour
{
    [SerializeField] BoxCollider2D col2d;
    [SerializeField] Rigidbody2D rg2d;
    [SerializeField] SpriteRenderer image;
    [SerializeField] ParticleSystem ps;

    GameManager gm;
    Vector2 originPos;
    bool isCrush = false;

    private void Start()
    {
        gm = GameManager.Instance;
        originPos = transform.position;
    }

    private void Update()
    {
        if (transform.position.y < gm.cameraCollider.points[2].y)
        {
            StartCoroutine(Reset());
        }
    }

    private IEnumerator Reset()
    {
        image.color = new Color(255, 255, 255, 0f);
        col2d.enabled = false;
        rg2d.velocity = Vector2.zero;
        isCrush = false;
        transform.position = originPos;

        yield return new WaitForSeconds(1.5f);

        image.color = new Color(255, 255, 255, 255f);
        col2d.enabled = true;
    }

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
                StartCoroutine(Reset());
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
