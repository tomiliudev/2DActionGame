using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Create WeaponImageData", fileName = "WeaponImageData")]
[Serializable]
public sealed class WeaponImageScriptableObject : ScriptableObject
{
    [SerializeField] public Sprite[] weaponSpriteList;
}