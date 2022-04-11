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

    public static AudioManager Instance { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
        musicSlider.value = sounds[0].source.volume;
        sfxSlider.value = (float)0.5;//ounds[2].source.volume;
        muteToggle.isOn = false;

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
    public void Pause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Pause();
    }
    public void UnPause(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.UnPause();
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
    public void StopLoop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.loop = false;
    }
    public void SetMusicVolume()
    {
        sounds[0].source.volume = musicSlider.value;
        sounds[1].source.volume = musicSlider.value;
    }
    public float GetMusicVolume()
    {
        return musicSlider.value;
    }

    public void SetSFXVolume()
    {
        for (int i = 2; i < sounds.Length; i++)
        {
            sounds[i].source.volume = sfxSlider.value;
        }
    }

    public float GetSFXVolume()
    {
        return sfxSlider.value;
    }

    public void ToggleMute()
    {
        if (muteToggle.isOn)
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                savedVolumes[i] = sounds[i].source.volume;
                sounds[i].source.mute = true;
            }
        }
        else
        {
            for (int i = 0; i < sounds.Length; i++)
            {
                sounds[i].source.mute = false; //savedVolumes[i];
            }
        }
    }

    public bool isMute()
    {
        return muteToggle.isOn;
    }
}
