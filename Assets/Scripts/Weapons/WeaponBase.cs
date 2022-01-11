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
            PlayerPrefsUtility.AddToJsonList("weaponList", weaponInfo, weaponInfo._isMultiple);

            float yPos = transform.position.y;
            Hashtable hash = new Hashtable();
            hash.Add("y", yPos + 1.5f);
            hash.Add("time", 0.5f);
            hash.Add("oncomplete", "OnComplete");
            iTween.MoveTo(gameObject, hash);
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
    public e_WeaponType _type;
    public bool _isMultiple;

    public string TypeName()
    {
        return _type.ToString();
    }
}
