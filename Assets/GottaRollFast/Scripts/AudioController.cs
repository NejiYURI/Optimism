using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;
    private AudioSource m_AudioSource;

    private void Awake()
    {
        if (AudioController.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip audioClip, float Volume = 1, bool overlapping = true)
    {
        if (audioClip == null | m_AudioSource == null) return;
        if (!this.m_AudioSource.isPlaying | overlapping)
        {
            this.m_AudioSource.PlayOneShot(audioClip, Volume);
        }
    }
}
