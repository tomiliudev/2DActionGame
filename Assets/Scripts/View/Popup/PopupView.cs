using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum e_PopupName
{
    none,
    equipPopup,
    gameOverPopup,
    shopPopup,
    stageClearPopup,
    settingPopup,
}

public sealed class PopupView : MonoBehaviour
{
    [SerializeField] GameObject BackMaskImage;
    [SerializeField] PopupBase[] popupList;

    List<PopupBase> activePopupList = new List<PopupBase>();

    public void ShowPopup(e_PopupName popupName)
    {
        var popup = popupList.First(x => x.PopupName == popupName);
        if (popup != null)
        {
            popup = Instantiate(popup, transform, false);
            popup.transform.localPosition = new Vector3(0f, 580f, 0f);
            popup.PopupAnimation();
            BackMaskImage.SetActive(true);

            activePopupList.Add(popup);
        }
    }

    public void ClosePopup(e_PopupName popupName, Action callback = null)
    {
        var popup = activePopupList.FirstOrDefault(x => x.PopupName == popupName);
        if (popup != null)
        {
            StartCoroutine(popup.ClosePopup(callback));
        }
    }

    public void SwitchOffMask()
    {
        BackMaskImage.SetActive(false);
    }
}
