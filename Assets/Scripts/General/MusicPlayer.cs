using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    [SerializeField]
    private float masterVolume = 1.0f;


    private void Update()
    {
        AudioListener.volume = masterVolume;
    }
}
