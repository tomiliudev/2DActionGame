using System;
using System.Linq;
using UnityEngine;

public sealed class MiniGame1 : MonoBehaviour
{
    [SerializeField] GameObject gauge;
    [SerializeField] GameObject needle;
    [SerializeField] BasePenalty[] penaltys;

    int successCount = 0;

    float xPos = 0f;
    float needleSpeed = 2f;
    float needleMin = -0.66f;
    float needleMax = 0.66f;
    float gaugeSizeMin = 0.07f;
    float gaugeSizeMax = 0.5f;

    MiniGame1Gauge miniGame1Gauge;
    private void Start()
    {
        miniGame1Gauge = gauge.GetComponent<MiniGame1Gauge>();
        SetGaugeSize();
    }

    // 所持金をへらす
    // 残り時間をへらす
    // HPをへらす

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (miniGame1Gauge.IsHit)
            {
                successCount++;
                if (successCount >= 3)
                {
                    // 3回連続で成功したら
                    Destroy(gameObject);

                    // ドア開く
                    GetComponentInParent<Door>().DoAnime(true,
                        () =>
                        {
                            // ステージクリア
                            //GameManager.Instance.IsGameClear = true;
                            GameManager.Instance.stageUiView.SwitchOnBlackMask();
                        }
                    );
                }
            }
            else
            {
                // 失敗したらリセット
                successCount = 0;

                // 画面を揺らす
                GameUtility.Instance.ShakeScreen(0.5f, 0.1f, 0.1f);

                penaltys.OrderBy(_ => Guid.NewGuid()).First().ExePenalty();
            }

            // ゲージのサイズ変更
            SetGaugeSize();
        }

        // 針の移動
        MoveNeedle();
    }

    private void MoveNeedle()
    {
        var pos = needle.transform.localPosition;

        xPos += needleSpeed * Time.deltaTime;

        if (xPos > needleMax)
        {
            xPos = needleMax;
            needleSpeed = -needleSpeed;
        }
        if (xPos < needleMin)
        {
            xPos = needleMin;
            needleSpeed = -needleSpeed;
        }

        needle.transform.localPosition = new Vector3(xPos, pos.y, pos.z);
    }

    private void SetGaugeSize()
    {
        var gaugeSize = gauge.GetComponent<SpriteRenderer>().size;
        gauge.GetComponent<SpriteRenderer>().size = new Vector2(UnityEngine.Random.Range(gaugeSizeMin, gaugeSizeMax), gaugeSize.y);
    }
}
