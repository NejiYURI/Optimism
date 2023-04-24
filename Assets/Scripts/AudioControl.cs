using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    public AudioSource CarSound;

    public AudioSource SoundContorl;

    public static AudioControl instance;
    private void Awake()
    {
        instance = this;
    }

    public void SetCarSound(float Ratio)
    {
        if (CarSound == null) return;
        CarSound.volume = Mathf.Lerp(0f, 0.8f, Ratio);
        CarSound.pitch = Mathf.Lerp(1f, 3f, Ratio);
    }

    public void PlaySound(AudioClip audioClip, float Volume = 1, bool overlapping = true)
    {
        if (audioClip == null | SoundContorl == null) return;
        if (!this.SoundContorl.isPlaying | overlapping)
        {
            this.SoundContorl.PlayOneShot(audioClip, Volume);
        }
    }
}
