using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioMixerController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
    public void muteMusic()
    {
        audioMixer.SetFloat("volume", -80);
    }
    public void unmuteMusic()
    {
        audioMixer.SetFloat("volume", 0);
    }
}
