using System;
using UnityEngine;
using UnityEngine.UI;

public class Stage1UiView : MonoBehaviour
{
    [Header("カウントダウン秒")] [SerializeField] Text countDownTime;

    public bool IsGameClear { get; set; }

    float countDownSec = 10f;
    public int CountDownSec
    {
        get { return (int)countDownSec; }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        OnCountDown();
    }

    private void OnCountDown()
    {
        if (IsGameClear) return;
        if (countDownSec > 0f)
        {
            countDownSec -= Time.deltaTime;
        }
        else
        {
            countDownSec = 0f;
        }
        countDownTime.text = new TimeSpan(0, 0, (int)countDownSec).ToString(@"mm\:ss");
    }
}
