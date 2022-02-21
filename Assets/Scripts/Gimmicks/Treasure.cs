using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class Treasure : MonoBehaviour
{
    [SerializeField] Animator treasureAnimator;
    [SerializeField] Animator speechBubbleAnimator;
    [SerializeField] GameObject objectPrefab;
    [SerializeField] bool isOneTimeOnly;

    public bool IsOpened
    {
        get; private set;
    }

    private void Start()
    {
        // 一回限りの宝箱
        if (isOneTimeOnly) IsOpened = PlayerPrefsUtility.Load(GetTreasureKeyName(), 0) == 1;
        if (IsOpened) treasureAnimator.SetTrigger("open");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.PlayerTag)
        {
            if (!IsOpened)
            {
                speechBubbleAnimator.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.PlayerTag)
        {
            speechBubbleAnimator.gameObject.SetActive(false);
        }
    }

    public void Open()
    {
        if (IsOpened) return;
        IsOpened = true;

        // 一回限りの宝箱
        if (isOneTimeOnly) PlayerPrefsUtility.Save(GetTreasureKeyName(), 1);

        treasureAnimator.SetTrigger("open");
        speechBubbleAnimator.gameObject.SetActive(false);

        // ドロップアイテム上昇アニメーション
        DropItemRiseAnime();
    }

    GameObject dropItem;
    private void DropItemRiseAnime()
    {
        if (objectPrefab != null)
        {
            dropItem = Instantiate(objectPrefab, transform.parent, false);
            dropItem.transform.position = transform.position;
            float yPos = dropItem.transform.position.y;
            Hashtable hash = new Hashtable();
            hash.Add("y", yPos + 1.5f);
            hash.Add("time", 0.5f);
            hash.Add("oncomplete", "OnRiseFinished");
            hash.Add("oncompletetarget", gameObject);
            iTween.MoveTo(dropItem, hash);
        }
    }

    private void OnRiseFinished()
    {
        StartCoroutine(DropItemHomingPlayerAnime());
    }

    private IEnumerator DropItemHomingPlayerAnime()
    {
        while (dropItem != null)
        {
            yield return new WaitForFixedUpdate();

            if (dropItem == null) yield break;

            Vector2 playerPos = GameManager.Instance.player.transform.position;
            iTween.MoveUpdate(
                dropItem,
                iTween.Hash(
                    "time", 0.5f,
                    "x", playerPos.x,
                    "y", playerPos.y
                )
            );
        }
    }

    private string GetTreasureKeyName()
    {
        return GameUtility.Instance.GetCurrentSceneName() + gameObject.name;
    }
}
