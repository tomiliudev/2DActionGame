

using UnityEngine;

public sealed class BombItem : ItemBase
{
    public override void Use()
    {
        Debug.Log(string.Format("{0}を使用する", base.itemInfo._type));
    }
}
