using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Rigidbody2D rb2D;
    [SerializeField] CircleCollider2D cc2D;
    [SerializeField] float moveSpeed;
    [SerializeField] float gravity;
    [SerializeField] EnemyCollisionCheck ecc;
    [SerializeField] ObjectCollision objectCollision;

    private bool isRight;
    private bool isDead;

    void FixedUpdate()
    {
        if (isDead) return;
        if (objectCollision.isPlayerStepOn)
        {
            isDead = true;
            rb2D.velocity = new Vector2(0, -gravity);
            cc2D.enabled = false;
        }

        if (sr.isVisible)
        {
            if (ecc.IsOn)
            {
                isRight = !isRight;
            }

            float xVector = -1f;
            if (isRight)
            {
                xVector = 1f;
                transform.localScale = new Vector3(-10f, 10f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(10f, 10f, 1f);
            }
            rb2D.velocity = new Vector2(xVector * moveSpeed, -gravity);
        }
        else
        {
            rb2D.Sleep();
        }
    }
}
