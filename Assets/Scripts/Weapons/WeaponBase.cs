using System;
using System.Collections;
using UnityEngine;

public enum e_WeaponType
{
    none,
    bow
}

public abstract class WeaponBase : MonoBehaviour
{
    [SerializeField] WeaponInfo weaponInfo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // 獲得データを保存する
            PlayerPrefsUtility.AddToJsonList("weaponList", weaponInfo, weaponInfo.IsMultiple);

            float yPos = transform.position.y;
            Hashtable hash = new Hashtable();
            hash.Add("y", yPos + 1.5f);
            hash.Add("time", 0.5f);
            hash.Add("oncomplete", "OnComplete");
            iTween.MoveTo(gameObject, hash);

            if (GameConfig.GetEquippedWeapon().Type == e_WeaponType.none)
            {
                PlayerPrefsUtility.SaveToJson(GameConfig.EquippedWeapon, weaponInfo);
                var gm = GameManager.Instance;
                gm.stageUiView.SetWeaponIconImage(weaponInfo);
                gm.stageUiView.ShowUseWeaponButton();
            }
        }
    }

    private void OnComplete()
    {
        Destroy(gameObject);
    }
}

[Serializable]
public class WeaponInfo : IEquipObjectInfo
{
    public WeaponInfoScriptableObject weaponInfoData;

    public e_WeaponType Type
    {
        get
        {
            return weaponInfoData != null ? weaponInfoData.type : e_WeaponType.none;
        }
    }

    public int Price
    {
        get
        {
            return weaponInfoData != null ? weaponInfoData.price : 0;
        }
    }

    public bool IsMultiple
    {
        get
        {
            return weaponInfoData != null ? weaponInfoData.isMultiple : false;
        }
    }
}
