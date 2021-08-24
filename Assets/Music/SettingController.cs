using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource _audioSource;
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume",volume);
    }
    public void muteMusic()
    {
        audioMixer.SetFloat("volume", -80);
    }
    public void unmuteMusic()
    {
        audioMixer.SetFloat("volume", -15);
    }
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    
    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}
