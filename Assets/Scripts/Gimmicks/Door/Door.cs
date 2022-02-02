using System;
using System.Collections;
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
            //StartCoroutine(DoAnime(false));
        }
    }

    public void OpenDoorAnimation()
    {
        if (!IsOnDoor) return;

        doorArrow.SetActive(false);

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

        Camera mainCamera = Camera.main;
        iTween.MoveTo(
            miniGameObj
            , iTween.Hash(
                "time", 1f,
                "x", mainCamera.transform.position.x,
                "y", mainCamera.transform.position.y
            )
        );

        GameManager.Instance.CurrentGameMode = e_GameMode.MiniGame;
    }


    public void DoAnime(bool flag, Action callback = null) {
        StartCoroutine(DoAnimeCo(flag, callback));
    }

    private IEnumerator DoAnimeCo(bool flag, Action callback = null)
    {
        doorAnimator.SetBool("isOpen", flag);
        
        yield return new WaitForSeconds(1f);

        if (callback != null) callback();
    }
}
