using System.Collections;
using UnityEngine;

public class MetalBase : MonoBehaviour
{
    GameManager gm;

    Hashtable hash;

    float distance = 0f;
    float speed = 0.008f;

    private void Start()
    {
        gm = GameManager.Instance;

        hash = new Hashtable();
        hash["time"] = 0.5f;
        hash.Add("easeType", iTween.EaseType.linear);
    }

    private void Update()
    {
        Adsorb();
    }

    /// <summary>
    /// 吸着する
    /// </summary>
    private void Adsorb()
    {
        if (gm != null && gm.player != null)
        {
            if (gm.equippedItem == e_EquipItemType.magnet)
            {
                var hit = Physics2D.Raycast(transform.position, gm.player.transform.position - transform.position, 2f, 1 << LayerMask.NameToLayer("Player"));
                if (hit.collider != null)
                {
                    distance = Vector2.Distance(transform.position, gm.player.transform.position);

                    // hash.Addだとキーすでにあるからエラーになるけど、上書きしたい場合はこのように指定
                    hash["position"] = gm.player.transform.position;
                    iTween.MoveUpdate(gameObject, hash);
                }
            }
        }
    }
}
