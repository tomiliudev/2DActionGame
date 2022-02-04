using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Create ItemImageData", fileName = "ItemImageData")]
[Serializable]
public sealed class ItemImageScriptableObject : ScriptableObject
{
    [SerializeField] public Sprite[] itemSpriteList;
}