using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public sealed class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    AudioSource audioSource;
    AudioSource bgmAudio;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (bgmAudio == null)
        {
            bgmAudio = gameObject.AddComponent<AudioSource>();
            AudioClip bgmClip = Resources.Load<AudioClip>("Bgm/SuspiciousCave");
            bgmAudio.clip = bgmClip;
            bgmAudio.loop = true;
        }
    }

    public void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void PlayBgm()
    {
        bgmAudio.Play();
    }

    public void StopBgm()
    {
        bgmAudio.Stop();
    }
}
