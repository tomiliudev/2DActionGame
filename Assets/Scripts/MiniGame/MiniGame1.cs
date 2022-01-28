using Cinemachine;
using UnityEngine;

public class MiniGame1 : MonoBehaviour
{
    [SerializeField] GameObject gauge;
    [SerializeField] GameObject needle;

    int successCount = 0;

    float xPos = 0f;
    float needleSpeed = 2f;
    float needleMin = -0.66f;
    float needleMax = 0.66f;
    float gaugeSizeMin = 0.07f;
    float gaugeSizeMax = 0.5f;

    Camera mainCamera;
    private void Start()
    {
        SetGaugeSize();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isHit)
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
        gauge.GetComponent<SpriteRenderer>().size = new Vector2(Random.Range(gaugeSizeMin, gaugeSizeMax), gaugeSize.y);
    }

    bool isHit = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.MiniGameNeedleTag)
        {
            isHit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.MiniGameNeedleTag)
        {
            isHit = false;
        }
    }
}
