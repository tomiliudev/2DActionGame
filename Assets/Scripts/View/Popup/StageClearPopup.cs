using System.Linq;
using UnityEngine;

public sealed class StageClearPopup : PopupBase
{
    [SerializeField] GetObjParts getObjPartsPrefab;
    [SerializeField] Transform parent;
    [SerializeField] ItemImageScriptableObject itemImageData;

    public void Start()
    {
        int getPoints = gm.sceneController.GetPoints;
        var coinParts = Instantiate(getObjPartsPrefab, parent, false);
        coinParts.SetObjImage();
        coinParts.SetObjNum(getPoints);

        var getItemGroup = gm.sceneController.GetItems.GroupBy(x => x.Type);
        foreach (var itemGroup in getItemGroup)
        {
            var getObj = Instantiate(getObjPartsPrefab, parent, false);
            getObj.SetObjImage(itemImageData.itemSpriteList[(int)itemGroup.First().Type - 1]);
            getObj.SetObjNum(itemGroup.Count());
        }
    }
}