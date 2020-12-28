using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public AudioMixer aMixer;

    public Sound[] sounds;
    public int lowestDeciblesBeforeMute = -20; //valor del sonido mas bajo en decibelios

    // Start is called before the first frame update
    void Start()
    {
        //Control de los Audios
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.volume = s.volume;
            s.source.outputAudioMixerGroup = s.mixerGroup;
        }

        PlaySound("Music");

        //Control de Volumen
        //Obtenemos los valores de volumen de las Preferencias del jugador
        int generalVolume = PlayerPrefs.GetInt("GeneralVolume", 100);
        aMixer.SetFloat("GeneralVolume", AdjustVolume(generalVolume));

        int soundVolume = PlayerPrefs.GetInt("SoundVolume", 100);
        aMixer.SetFloat("SoundVolume", AdjustVolume(soundVolume));

        int musicVolume = PlayerPrefs.GetInt("MusicVolume", 100);
        aMixer.SetFloat("MusicVolume", AdjustVolume(musicVolume));
    }
    public float AdjustVolume(int volume)
    {
        // Cambiamos el valor de entrada por una salida ajustada a los decibelios
        float adjustedVolume = lowestDeciblesBeforeMute + (-lowestDeciblesBeforeMute / 5 * volume / 20);
        return adjustedVolume;
    }

    public void PlaySound(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                s.source.Play();
            }
        }
    }

    public void StopSound(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name == name)
            {
                s.source.Stop();
            }
        }
    }

    public void StopAllSounds()
    {
        foreach (Sound s in sounds)
        {  
            s.source.Stop();  
        }
    }

    public void KeepPlayingSound(string name)
    {
        foreach (Sound s in sounds)
        {
            if (s.name != name)
            {
                s.source.Stop();
            }
        }
    }

}
