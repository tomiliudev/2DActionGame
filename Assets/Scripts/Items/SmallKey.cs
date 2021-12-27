using UnityEngine;

public sealed class SmallKey : ItemBase
{
    public override void Use()
    {
        Debug.Log(string.Format("{0}を使用する", base.itemInfo._type));
    }
}
