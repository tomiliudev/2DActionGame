using UnityEngine;

public sealed class PlayButtonObj : MonoBehaviour
{
    [SerializeField] AudioClip shakeSE;
    public void MoveAnime()
    {
        SoundManager.Instance.Play(shakeSE);

        Transform trans = gameObject.transform;
        iTween.MoveTo(gameObject,
            iTween.Hash(
                "x", trans.position.x + 3,
                "time", 5f,
                //"easeType", iTween.EaseType.easeInOutBack,
                "oncomplete", "OnCloseAnimationFinished"
            )
        );
    }
}
