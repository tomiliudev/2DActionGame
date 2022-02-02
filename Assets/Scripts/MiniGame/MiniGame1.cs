using System;
using System.Linq;
using Cinemachine;
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

    Camera mainCamera;
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
                Debug.Log("あたり！！！");
                successCount++;
                Debug.Log("成功カウント" + successCount);
            }
            else
            {
                Debug.Log("残念！！！");
                successCount = 0;
                Debug.Log("成功カウント" + successCount);
                mainCamera = Camera.main;
                mainCamera.GetComponent<CinemachineBrain>().enabled = false;
                iTween.ShakePosition(
                    mainCamera.gameObject,
                    iTween.Hash(
                        "time", 0.5f,
                        "x", 0.1f,
                        "y", 0.1f,
                        "oncomplete", "OnShakeCameraFinish"
                    )
                );

                penaltys.OrderBy(_ => Guid.NewGuid()).First().ExePenalty();
            }

            // ゲージのサイズ変更
            SetGaugeSize();
        }

        // 針の移動
        MoveNeedle();
    }

    private void OnShakeCameraFinish()
    {
        mainCamera.GetComponent<CinemachineBrain>().enabled = true;
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
