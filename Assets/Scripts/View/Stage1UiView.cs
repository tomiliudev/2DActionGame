using System;
using UnityEngine;
using UnityEngine.UI;

public class Stage1UiView : MonoBehaviour
{
    [Header("経過時間")][SerializeField] Text elapsedTime;

    float sec = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        sec += Time.deltaTime;
        elapsedTime.text = new TimeSpan(0, 0, (int)sec).ToString(@"mm\:ss");
    }
}
