using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer sr;
    [SerializeField] protected Rigidbody2D rb2D;
    [SerializeField] protected CircleCollider2D cc2D;
    [SerializeField] protected CapsuleCollider2D capsuleCollider2D;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float gravity;
    [SerializeField] protected EnemyCollisionCheck wallCollisionCheck;
    [SerializeField] protected EnemyCollisionCheck groundCollisionCheck;
    [SerializeField] protected ObjectCollision objectCollision;
    [SerializeField] protected Animator animator;


    [SerializeField] private ContactFilter2D filter2d = default;

    protected const string playerTag = "Player";
    protected const string playerLayer = "Player";

    GameManager gm;

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
        gm = GameManager.Instance;
    }

    private void Start()
    {
        // protected やpublicをつけて、子クラスのStartでbase.Startで呼ばない限り、継承元のStartは呼ばれないみたい
        // しかも呼ばれる回数はゲームシーンにある子の数＋親の数になる
        // Awakeも同じ
    }

    protected void FixedUpdate()
    {
        if (!gm.IsInitialized) return;

        // 敵が死んだら何もしない
        if (IsEnemyDead()) isCanMove = false;

        // ゲームクリアしたら何もしない
        if (gm.IsGameClear)
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
    /// プレイヤーとの間に障害物（床や壁）があるとヒットしない
    /// </summary>
    /// <returns></returns>
    protected bool IsHitPlayer()
    {
        RaycastHit2D[] results = new RaycastHit2D[2];
        Physics2D.Raycast(transform.position, playerVector, filter2d, results, 5f);
        var tran = results[0].transform;
        if (tran != null)
        {
            return playerLayer == LayerMask.LayerToName(tran.gameObject.layer);
        }

        return false;
    }
}
