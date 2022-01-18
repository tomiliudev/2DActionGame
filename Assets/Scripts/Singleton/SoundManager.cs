using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public sealed class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
