using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class DataManager : SingletonMonoBehaviour<DataManager>
{
    [SerializeField] ItemInfoScriptableObject[] itemInfoDatas;
    [SerializeField] Sprite[] thumbnails;

    public List<ItemInfoScriptableObject> GetItemInfoDatas()
    {
        if (itemInfoDatas == null || itemInfoDatas.Count() <= 0)
        {
            // FindObjectsOfTypeAllでやると何故かロードされたデータが破損してしまう
            //itemInfoDatas = Resources.FindObjectsOfTypeAll<ItemInfoScriptableObject>();
            itemInfoDatas = Resources.LoadAll<ItemInfoScriptableObject>("ItemData/");
        }
        
        return itemInfoDatas.ToList();
    }

    public Sprite GetTargetThumbnail(string thumbnailName)
    {
        string prefix = "Screenshot";
        if (thumbnails == null || thumbnails.Count() <= 0)
        {
            thumbnails = Resources.LoadAll<Sprite>("Thumbnails/");
        }
        string fileName = prefix + thumbnailName;        
        return thumbnails.FirstOrDefault(x => x.name == fileName);
    }
}
