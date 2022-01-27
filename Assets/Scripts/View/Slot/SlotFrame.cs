using UnityEngine;
using UnityEngine.UI;

public sealed class SlotFrame : MonoBehaviour
{
    [SerializeField] Text numText;

    public void SetNumText(int num = 0)
    {
        this.numText.text = num.ToString();
        numText.gameObject.SetActive(num > 0);
    }

    public void AddNum(int num)
    {
        int currentNum = 0;
        if(int.TryParse(numText.text, out currentNum))
        {
            SetNumText(currentNum + num);
        }
    }
}
