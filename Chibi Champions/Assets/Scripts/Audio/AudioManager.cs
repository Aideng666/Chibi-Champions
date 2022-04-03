using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Toggle muteToggle;

    float[] savedVolumes;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }

        savedVolumes = new float[sounds.Length];

        musicSlider.onValueChanged.AddListener(delegate { SetMusicVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { SetSFXVolume(); });
        muteToggle.onValueChanged.AddListener(delegate { ToggleMute(); });
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public bool IsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        return s.source.isPlaying;
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }

    public void Loop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.loop = true;
    }

    public void SetMusicVolume()
    {
        sounds[0].source.volume = musicSlider.value;
    }

    public void SetSFXVolume()
    {
        for (int i = 1; i < sounds.Length; i++)
        {
            sounds[i].source.volume = sfxSlider.value;
        }
    }

    public void ToggleMute()
    {
        if (muteToggle.isOn)
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                savedVolumes[i] = sounds[i].source.volume;
                sounds[i].source.volume = 0;
            }
        }
        else
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                sounds[i].source.volume = savedVolumes[i];
            }
        }
    }
}
