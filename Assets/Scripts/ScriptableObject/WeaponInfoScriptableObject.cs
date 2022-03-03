using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/Create WeaponInfoData", fileName = "WeaponInfoData")]
[Serializable]
public class WeaponInfoScriptableObject : ScriptableObject
{
    [SerializeField] public e_WeaponType type;
    [SerializeField] public int price;
    [SerializeField] public bool isMultiple;
}