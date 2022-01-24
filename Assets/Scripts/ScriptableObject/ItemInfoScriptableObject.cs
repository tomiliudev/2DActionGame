using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Create ItemInfoData", fileName = "ItemInfoData")]
[Serializable]
public class ItemInfoScriptableObject : ScriptableObject
{
    [SerializeField] public e_ItemType type;
    [SerializeField] public int price;
    [SerializeField] public bool isMultiple;
}