using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public sealed class UseBombItem : UseItemBase
{
    [SerializeField] Text countDown;
    [SerializeField] Explosion explosionAnimator;
    public override void Use()
    {
        Debug.Log(string.Format("{0}を使用する", base.itemInfo._type));
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
        Destroy(gameObject);
    }
}
