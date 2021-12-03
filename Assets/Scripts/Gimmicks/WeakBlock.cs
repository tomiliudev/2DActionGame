using System.Collections;
using System.Linq;
using UnityEngine;

public class WeakBlock : MonoBehaviour
{
    [SerializeField] Rigidbody2D rg2d;
    [SerializeField] Animator animator;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.collider.tag)
        {
            case "Player":
                StartCoroutine(CrushAnimation());
                break;
            case "Ground":
                animator.SetTrigger("crush");
                Destroy(gameObject);
                break;
        }
    }

    IEnumerator CrushAnimation()
    {
        
        float shackTime = 1.5f;
        gameObject.ShakePosition(new Vector2(0.1f, 0f), shackTime, 0f);

        yield return new WaitForSeconds(shackTime);

        rg2d.velocity = new Vector2(0f, -3f);
    }
}
