using UnityEngine;

public sealed class Coin : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSe;
    [SerializeField] GameObject score;
    [SerializeField] int point;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SoundManager.Instance.Play(coinPickupSe);

            int fromPoint = PlayerPrefsUtility.Load(GameConfig.TotalPoint, 0);
            int toPoint = fromPoint + point;
            GameManager.Instance.stageUiView.UpdateTotalPointView(fromPoint, toPoint);
            PlayerPrefsUtility.Save(GameConfig.TotalPoint, toPoint);

            score.SetActive(true);
            Destroy(gameObject);
        }
    }
}
