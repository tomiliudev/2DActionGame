using UnityEngine;
using UnityEngine.UI;

public sealed class GetObjParts : MonoBehaviour
{
    [SerializeField] Image objImage;
    [SerializeField] Sprite coinSprite;
    [SerializeField] Text objNumText;

    public void SetObjImage(Sprite objSprite = null)
    {
        if (objSprite == null) { objImage.sprite = coinSprite; }
        else { objImage.sprite = objSprite; }
    }

    public void SetObjNum(int objNum)
    {
        string objNumStr = objNum.ToString();
        if (objNum > 0)
        {
            objNumStr = "+" + objNumStr;
        }
        if(objNum < 0)
        {
            objNumStr = "-" + objNumStr;
        }
        objNumText.text = objNumStr;
    }
}
