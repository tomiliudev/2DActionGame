using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class SettingPopup : PopupBase
{
    [SerializeField] Slider bgmSlider;
    [SerializeField] Slider seSlider;

    SoundManager soundMg;

    // Start is called before the first frame update
    void Start()
    {
        soundMg = SoundManager.Instance;
        bgmSlider.value = soundMg.GetBgmVolume();
        seSlider.value = soundMg.GetSeVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBgmSliderChanged()
    {
        if (soundMg != null)
        {
            soundMg.SetBgmVolume(bgmSlider.value);
            PlayerPrefsUtility.Save(GameConfig.BgmVolumeKey, bgmSlider.value);
        }
    }

    public void OnSeSliderChanged()
    {
        if (seSlider != null)
        {
            soundMg.SetSeVolume(seSlider.value);
            PlayerPrefsUtility.Save(GameConfig.SeVolumeKey, seSlider.value);
        }
    }
}
