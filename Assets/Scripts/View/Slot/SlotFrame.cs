using UnityEngine;
using UnityEngine.UI;

public sealed class SlotFrame : MonoBehaviour
{
    [SerializeField] Text numText;

    public void SetNumText(int num)
    {
        this.numText.text = num.ToString();
        numText.gameObject.SetActive(num > 0);
    }
}
