using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public sealed class UseBombItem : UseItemBase
{
    [SerializeField] Text countDown;
    [SerializeField] Explosion explosionAnimator;
    [SerializeField] AudioClip explosionSe;

    public override void Use()
    {
        base.Use();
        var gm = GameManager.Instance;
        if (gm.player.IsOnRight)
        {
            transform.position = gm.player.transform.position + new Vector3(0.5f, 0f, 0f);
        }
        else
        {
            transform.position = gm.player.transform.position + new Vector3(-0.5f, 0f, 0f);
        }
    }

    private void Start()
    {
        StartCoroutine(ExeCountDown());
    }

    IEnumerator ExeCountDown()
    {
        int countDownNum = 3;
        countDown.text = countDownNum.ToString();
        yield return new WaitForSeconds(1f);
        while (countDownNum > 0)
        {
            countDownNum--;
            countDown.text = countDownNum.ToString();
            yield return new WaitForSeconds(1f);
        }

        if (countDownNum <= 0)
        {
            Explosion();
            yield break;
        }
    }

    // 爆発
    private void Explosion()
    {
        explosionAnimator.transform.parent = null;
        explosionAnimator.OnExplosion();
        SoundManager.Instance.Play(explosionSe);
        Destroy(gameObject);
    }
}
