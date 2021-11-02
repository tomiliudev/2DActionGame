using UnityEngine;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [SerializeField] Text text1;
    [SerializeField] Text text2;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Moved:
                    if (touch.deltaPosition.x > 0f)
                    {
                        text1.text = "右！！";
                    }
                    else if (touch.deltaPosition.x < 0f)
                    {
                        text1.text = "左！！";
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
