using System.Linq;
using UnityEngine;

public enum e_PopupName
{
    none,
    equipPopup,
    gameOverPopup,
    shopPopup
}

public sealed class PopupView : MonoBehaviour
{
    [SerializeField] GameObject BackMaskImage;
    [SerializeField] PopupBase[] popupList;
    [SerializeField] Transform popupPosition;
    public void ShowPopup(e_PopupName popupName)
    {
        var popup = popupList.First(x => x.PopupName == popupName);
        if (popup != null)
        {
            popup = Instantiate(popup, transform, false);
            popup.transform.localPosition = new Vector3(0f, 580f, 0f);
            popup.PopupAnimation();
            BackMaskImage.SetActive(true);
        }
    }

    public void SwitchOffMask()
    {
        BackMaskImage.SetActive(false);
    }
}
