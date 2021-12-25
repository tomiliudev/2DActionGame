using UnityEngine;
using UnityEngine.UI;

public class UiSlotBase : MonoBehaviour
{
    [SerializeField] protected Image iconImage;
    GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
    }

    public void OnFrameClicked()
    {
        if (gm == null) return;
        gm.popupView.ShowPopup(e_PopupName.equipPopup);
    }
}
