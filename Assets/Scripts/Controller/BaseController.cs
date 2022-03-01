using UnityEngine;

public class BaseController : MonoBehaviour, IPopupCloseButton
{
    protected GameManager gm;
    protected SoundManager soundMg;
    protected AdmobUtility admob;

    protected void Start()
    {
        soundMg = SoundManager.Instance;
        gm = GameManager.Instance;
        admob = AdmobUtility.Instance;

        float bgmVolume = soundMg.GetBgmVolume();
        soundMg.SetBgmVolume(bgmVolume);
        soundMg.PlayBgm();

        float seVolume = soundMg.GetSeVolume();
        soundMg.SetSeVolume(seVolume);

        admob.RequestInterstitial();
    }

    public class BaseInitData
    {

    }

    public virtual void Initialize(BaseInitData initData)
    {
        
    }

    public void OnPopupCloseButton(e_PopupName popupName)
    {
        gm.popupView.ClosePopup
        (
            popupName,
            () => { }
        );
    }
}
