using UnityEngine;

public sealed class TorchTrigger : MonoBehaviour
{
    [SerializeField] bool isAutoFire;
    [SerializeField] GameObject fireObj;

    public bool IsFlyOn { get; private set; }


    private void Start()
    {
        FireTorch(isAutoFire);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.FlyTag)
        {
            IsFlyOn = true;
            FireTorch(true);
        }
    }

    public void FireTorch(bool flag)
    {
        fireObj.SetActive(flag);
    }
}
