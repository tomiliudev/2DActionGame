using UnityEngine;

public sealed class Door : MonoBehaviour
{
    [SerializeField] GameObject doorArrow;
    [SerializeField] Animator doorAnimator;
    [SerializeField] GameObject miniGameObj;

    bool isOnDoor = false;
    public bool IsOnDoor
    {
        get { return isOnDoor; }
        private set { isOnDoor = value; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.PlayerTag)
        {
            IsOnDoor = true;
            doorArrow.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == GameConfig.PlayerTag)
        {
            IsOnDoor = false;
            doorArrow.SetActive(false);
            DoAnime(false);
        }
    }

    public void OpenDoorAnimation()
    {
        if (!IsOnDoor) return;
        DoAnime(true);
        doorArrow.SetActive(false);

        miniGameObj.transform.parent = null;
        miniGameObj.SetActive(true);
        iTween.RotateTo(
            miniGameObj
            , iTween.Hash(
                "time", 1f,
                "rotation", new Vector3(0f, 1800f, 1800f)
            )
        );
        iTween.ScaleTo(
            miniGameObj
            , iTween.Hash(
                "time", 1f,
                "x", 8f,
                "y", 8f
            )
        );
        iTween.MoveTo(
            miniGameObj
            , iTween.Hash(
                "time", 1f,
                "x", 0f,
                "y", 0f
            )
        );


        GameManager.Instance.CurrentGameMode = e_GameMode.MiniGame;
    }

    private void DoAnime(bool flag)
    {
        doorAnimator.SetBool("isOpen", flag);
    }
}
