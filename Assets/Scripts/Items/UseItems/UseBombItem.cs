using UnityEngine;

public sealed class UseBombItem : UseItemBase
{
    public override void Use()
    {
        Debug.Log(string.Format("{0}を使用する", base.itemInfo._type));
    }
}
