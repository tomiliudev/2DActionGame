using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class TorchTrigger : MonoBehaviour
{
    public bool IsFlyOn { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.FlyTag)
        {
            IsFlyOn = true;
        }
    }
}
