using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonObj : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveAnime()
    {
        Transform trans = gameObject.transform;
        iTween.MoveTo(gameObject,
            iTween.Hash(
                "x", trans.position.x + 3,
                "time", 15f,
                //"easeType", iTween.EaseType.easeInOutBack,
                "oncomplete", "OnCloseAnimationFinished"
            )
        );
    }
}
