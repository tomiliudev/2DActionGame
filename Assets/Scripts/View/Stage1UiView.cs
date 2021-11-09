using System;
using UnityEngine;
using UnityEngine.UI;

public class Stage1UiView : MonoBehaviour
{
    [Header("経過時間")][SerializeField] Text elapsedTime;

    DateTime startTime;
    TimeSpan elapsedTimeSpan;

    // Start is called before the first frame update
    void Start()
    {
        startTime = DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTimeSpan = DateTime.Now - startTime;
        elapsedTime.text = string.Format("{0}:{1}:{2}",
            elapsedTimeSpan.Hours.ToString("00"),
            elapsedTimeSpan.Minutes.ToString("00"),
            elapsedTimeSpan.Seconds.ToString("00")
        );
    }
}
