using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer sr;
    [SerializeField] protected Rigidbody2D rb2D;
    [SerializeField] protected CircleCollider2D cc2D;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float gravity;
    [SerializeField] protected EnemyCollisionCheck ecc;
    [SerializeField] protected ObjectCollision objectCollision;
    [SerializeField] protected Animator animator;

    public bool IsGameClear { get; set; }

    protected bool IsEnemyDead()
    {
        bool isDead = false;
        if (objectCollision.isPlayerStepOn)
        {
            isDead = true;
            rb2D.velocity = new Vector2(0f, -3f);
            cc2D.enabled = false;
            if (animator != null) animator.enabled = false;
        }
        return isDead;
    }
}
