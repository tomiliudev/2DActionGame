using System.Collections;
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
    [SerializeField] protected GameObject dropItem;
    [SerializeField] protected float boundHight;// プレイヤーが踏みつけた時にバウンドする力
    [SerializeField] private ContactFilter2D filter2d = default;

    protected GameManager gm;

    protected const string playerLayer = "Player";

    bool isDead = false;
    protected bool IsDead { get { return this.isDead; } }
    public float BoundHight { get { return boundHight; } }
    protected bool IsOnDamage { get; private set; }

    protected Vector3 playerVector;

    protected int Hp = 1;
    
    private void Awake()
    {
        gm = GameManager.Instance;

        // ステージレベルでHPが決まる
        Hp = gm.CurrentStageLevel + 1;
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
        playerVector = gm.player.transform.position - transform.position;
    }

    Coroutine updateIsOnDamageFlagCo = null;
    public void OnDamage()
    {
        Hp--;
        if (Hp <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("hit");

            if (updateIsOnDamageFlagCo != null) StopCoroutine(updateIsOnDamageFlagCo);
            updateIsOnDamageFlagCo = StartCoroutine(UpdateIsOnDamageFlag());
        }
    }

    private IEnumerator UpdateIsOnDamageFlag()
    {
        IsOnDamage = true;
        yield return new WaitForSeconds(0.5f);
        IsOnDamage = false;
    }

    // 撃破される
    private void Die()
    {
        if (Hp > 0) return;
        isDead = true;

        if (dropItem != null)
        {
            var dropItemObj = Instantiate(dropItem, transform.parent);
            dropItemObj.transform.position = transform.position;
        }

        // クリアレベルに応じてポイントを与える
        int point = gm.CurrentStageLevel;
        int toPoint = gm.sceneController.GetPoints + point;
        gm.stageUiView.UpdateTotalPointView(gm.sceneController.GetPoints, toPoint);
        gm.sceneController.GetPoints = toPoint;

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

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == GameConfig.PlayerTag)
        {
            if (gm.player.CheckCollisionDetectionWithEnemy(transform))
            {
                gm.player.OnDamage();
            }
            else
            {
                // プレイヤーに踏みつけられた
                OnDamage();
                gm.player.OnBound(boundHight);
            }
        }
    }

    protected bool IsDoFreeze(bool isFreeze = false)
    {
        bool _isFreeze =
            GameUtility.IsGamePause
            || IsOnDamage
            || isFreeze;

        if (_isFreeze)
        {
            rb2D.Sleep();
        }

        return _isFreeze;
    }
}
