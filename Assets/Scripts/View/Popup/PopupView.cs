using System.Linq;
using UnityEngine;

public enum e_PopupName
{
    none,
    equipPopup,
}

public class PopupView : MonoBehaviour
{
    [SerializeField] GameObject BackMaskImage;
    [SerializeField] PopupBase[] popupList;
    public void ShowPopup(e_PopupName popupName)
    {
        var popup = popupList.First(x => x.PopupName == popupName);
        if (popup != null)
        {
            popup = Instantiate(popup);
            popup.transform.SetParent(transform, false);
            BackMaskImage.SetActive(true);
        }
    }

    public void SwitchOffMask()
    {
        BackMaskImage.SetActive(false);
    }
}
