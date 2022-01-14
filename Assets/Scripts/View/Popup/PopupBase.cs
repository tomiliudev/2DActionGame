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

    public void OnCloseButtonClicked()
    {
        gm.popupView.SwitchOffMask();
        Destroy(gameObject);
    }
}
