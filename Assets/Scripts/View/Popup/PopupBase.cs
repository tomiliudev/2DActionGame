using UnityEngine;

public class PopupBase : MonoBehaviour
{
    [SerializeField] e_PopupName popupName = e_PopupName.none;
    public e_PopupName PopupName { get { return popupName; } }

    public void OnCloseButtonClicked()
    {
        Destroy(gameObject);
    }
}
