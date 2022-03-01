using System;
using System.Collections;
using UnityEngine;

public class PopupBase : MonoBehaviour
{
    [SerializeField] e_PopupName popupName = e_PopupName.none;
    public e_PopupName PopupName { get { return popupName; } }

    protected GameManager gm;

    private void Awake()
    {
        gm = GameManager.Instance;
    }

    public void PopupAnimation()
    {
        iTween.MoveTo(gameObject, iTween.Hash("y", 30f, "time", 1f, "easeType", iTween.EaseType.easeOutElastic, "isLocal", true));
    }

    private void CloseAnimation()
    {
        iTween.MoveTo(gameObject,
            iTween.Hash(
                "y", 580f,
                "time", 1f,
                "easeType", iTween.EaseType.easeInOutBack,
                "oncomplete", "OnCloseAnimationFinished",
                "isLocal", true
            )
        );
    }

    bool isClosed = false;
    private void OnCloseAnimationFinished()
    {
        isClosed = true;
        gm.popupView.SwitchOffMask();
    }

    public IEnumerator ClosePopup(Action callBack = null)
    {
        CloseAnimation();
        yield return new WaitUntil(() => isClosed);
        if (callBack != null) callBack();
        Destroy(gameObject);
    }
}
