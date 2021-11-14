using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer sr;
    [SerializeField] protected Rigidbody2D rb2D;
    [SerializeField] protected CircleCollider2D cc2D;
    [SerializeField] protected CapsuleCollider2D capsuleCollider2D;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float gravity;
    [SerializeField] protected EnemyCollisionCheck ecc;
    [SerializeField] protected ObjectCollision objectCollision;
    [SerializeField] protected Animator animator;

    protected const string playerTag = "Player";

    public bool IsGameClear { get; set; }

    private bool isCanMove = true;
    protected bool IsCanMove
    {
        get { return this.isCanMove; }
    }

    protected Transform playerTransform;
    protected Vector3 playerVector;

    private void Awake()
    {
        playerTransform = GameObject.FindWithTag(playerTag).transform;
    }

    void Start()
    {
        
    }

    protected void FixedUpdate()
    {
        // 敵が死んだら何もしない
        if (IsEnemyDead()) isCanMove = false;

        // ゲームクリアしたら何もしない
        if (IsGameClear)
        {
            rb2D.velocity = Vector2.zero;
            isCanMove = false;
        }

        // プレイヤーのベクトル
        playerVector = playerTransform.position - transform.position;
    }

    protected bool IsEnemyDead()
    {
        bool isDead = false;
        if (objectCollision.isPlayerStepOn)
        {
            isDead = true;
            rb2D.velocity = new Vector2(0f, -3f);
            if (cc2D != null) cc2D.enabled = false;
            if (capsuleCollider2D != null) capsuleCollider2D.enabled = false;
            if (animator != null) animator.enabled = false;
        }
        return isDead;
    }

    /// <summary>
    /// プレイヤーへRayを飛ばす
    /// </summary>
    /// <returns></returns>
    protected RaycastHit2D GetPlayerHit()
    {
        return Physics2D.Raycast(transform.position, playerVector, 5f, LayerMask.GetMask("Player"));
    }
}
