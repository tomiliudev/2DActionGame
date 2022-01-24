using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public sealed class DataManager : SingletonMonoBehaviour<DataManager>
{
    [SerializeField] ItemInfoScriptableObject[] itemInfoDatas;

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
}
