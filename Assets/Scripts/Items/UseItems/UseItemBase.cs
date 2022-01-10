using System.Collections;
using UnityEngine;

public abstract class UseItemBase : MonoBehaviour
{
    [SerializeField] protected ItemInfo itemInfo;

    protected GameManager gm;
    private void Awake()
    {
        gm = GameManager.Instance;
    }

    // 上昇するアニメーション
    protected void RiseAnimation()
    {
        transform.position = gm.player.transform.position;
        float yPos = transform.position.y;
        Hashtable hash = new Hashtable();
        hash.Add("y", yPos + 1.5f);
        hash.Add("time", 0.5f);
        hash.Add("oncomplete", "OnRiseAnimationComplete");
        iTween.MoveTo(gameObject, hash);
    }

    private void OnRiseAnimationComplete()
    {
        Destroy(gameObject);
    }

    // アイテムを使用する
    public abstract void Use();
}
