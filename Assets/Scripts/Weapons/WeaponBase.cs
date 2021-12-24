using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum e_WeaponType
{
    none,
    bow
}

public abstract class WeaponBase : MonoBehaviour
{
    GameManager gm;

    [SerializeField] WeaponInfo weaponInfo;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // 獲得データを保存する
            PlayerPrefsUtility.SaveJsonList("weaponList", weaponInfo);
            gm.equippedWeapon = weaponInfo.weaponType;

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
    public e_WeaponType weaponType;
    public Sprite weaponSprite;

    public Sprite GetSprite()
    {
        return weaponSprite;
    }
}
