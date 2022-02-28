using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected SoundManager soundMg;

    protected void Start()
    {
        soundMg = SoundManager.Instance;

        float bgmVolume = soundMg.GetBgmVolume();
        soundMg.SetBgmVolume(bgmVolume);
        soundMg.PlayBgm();

        float seVolume = soundMg.GetSeVolume();
        soundMg.SetSeVolume(seVolume);
    }

    public class BaseInitData
    {

    }

    public virtual void Initialize(BaseInitData initData)
    {
        
    }
}
