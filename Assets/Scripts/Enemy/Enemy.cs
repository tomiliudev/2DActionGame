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
    [SerializeField] protected Animator animator;
    [SerializeField] protected PickHeart pickHeart;
    [SerializeField] protected float boundHight;// プレイヤーが踏みつけた時にバウンドする力
    [SerializeField] private ContactFilter2D filter2d = default;

    GameManager gm;

    protected const string playerTag = "Player";
    protected const string playerLayer = "Player";

    bool isDead = false;
    protected bool IsDead { get { return this.isDead; } }
    public float BoundHight { get { return boundHight; } }

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

        // ゲームクリアしたら何もしない
        if (gm.IsGameClear)
        {
            rb2D.velocity = Vector2.zero;
        }

        // プレイヤーのベクトル
        playerVector = playerTransform.position - transform.position;
    }

    public void OnDamage()
    {
        isDead = true;

        var pickHeartObj = Instantiate(pickHeart, transform.parent);
        pickHeartObj.transform.position = transform.position;

        Destroy(gameObject);
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
