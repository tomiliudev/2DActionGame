using UnityEngine;

public sealed class Coin : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSe;
    [SerializeField] GameObject score;
    [SerializeField] int point;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.PlayerTag)
        {
            SoundManager.Instance.Play(coinPickupSe);

            GameManager gm = GameManager.Instance;
            int toPoint = gm.sceneController.GetPoints + point;
            gm.stageUiView.UpdateTotalPointView(gm.sceneController.GetPoints, toPoint);
            gm.sceneController.GetPoints = toPoint;

            score.SetActive(true);
            Destroy(gameObject);
        }
    }
}
