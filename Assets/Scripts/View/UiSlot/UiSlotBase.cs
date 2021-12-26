using UnityEngine;
using UnityEngine.UI;

public class UiSlotBase : MonoBehaviour
{
    [SerializeField] protected Image iconImage;
    GameManager gm;

    protected IEquipObjectInfo _equipInfo;

    // Start is called before the first frame update
    protected void Start()
    {
        gm = GameManager.Instance;
        SetIconImage(_equipInfo);
    }

    public void OnFrameClicked()
    {
        if (gm == null) return;
        gm.popupView.ShowPopup(e_PopupName.equipPopup);
    }

    public void SetIconImage<T>(T equipInfo) where T : IEquipObjectInfo
    {
        Sprite iconSprite = equipInfo.GetSprite();
        iconImage.sprite = iconSprite;
        iconImage.preserveAspect = true;
        iconImage.gameObject.SetActive(iconSprite != null);
    }
}
