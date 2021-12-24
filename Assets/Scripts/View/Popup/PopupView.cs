using System.Linq;
using UnityEngine;

public enum e_PopupName
{
    none,
    equipPopup,
}

public class PopupView : MonoBehaviour
{
    [SerializeField] PopupBase[] popupList;
    public void ShowPopup(e_PopupName popupName)
    {
        var popup = popupList.First(x => x.PopupName == popupName);
        if (popup != null)
        {
            popup = Instantiate(popup);
            popup.transform.SetParent(transform, false);
        }
    }
}
