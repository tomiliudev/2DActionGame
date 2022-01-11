using UnityEngine;
using UnityEngine.UI;

public class UiSlotBase : MonoBehaviour
{
    [SerializeField] protected Image iconImage;

    GameManager gm;

    protected void Awake()
    {
        gm = GameManager.Instance;
    }

    public void OnFrameClicked()
    {
        if (gm == null) return;
        gm.popupView.ShowPopup(e_PopupName.equipPopup);
    }
}
