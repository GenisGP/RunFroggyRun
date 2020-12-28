using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class AudioSlider : MonoBehaviour
{
    public AudioMixer aMixer;
    public enum AudioChannel { General, Sound, Music };
    public AudioChannel channel; 
    public Slider slider; 
    public float sValue; 
    public TMP_Text slText;
    public int lowestDeciblesBeforeMute = -20; //valor del sonido mas bajo en decibelios

    private void Awake() 
    {
        slider = GetComponent<Slider>(); 
    }

    // Start is called before the first frame update
    void Start()
    {
        //Obtenemos Los valores de volumen de las Preferencias del jugador
        int generalVolume = PlayerPrefs.GetInt("GeneralVolume", 100); 
        int soundVolume = PlayerPrefs.GetInt("SoundVolume", 100); 
        int musicVolume = PlayerPrefs.GetInt("MusicVolume", 100);

        //Dependiendo del grupo de destino del Audio Mixer actualizaremos un slider u otro en base a los datos recogidos de playerprefs
        switch (channel)
        {
            case AudioChannel.General: 
                slider.value = generalVolume / 100f; 
                break;
            case AudioChannel.Sound: 
                slider.value = soundVolume / 100f; 
                break;
            case AudioChannel.Music: 
                slider.value = musicVolume / 100f; 
                break;
        }

        UpdateSoundLvl();
    }

    public void UpdateSoundLvl()
    { 
        int sValue = (int)(slider.value * 100); 
        SetVolume(channel, sValue); 
        slText.text = sValue + " / " + "100"; 
    }

    public void SetVolume(AudioChannel channel, int volume)
    {
        // Cambiamos el valor de entrada por una salida ajustada a los decibelios
        float adjustedVolume = lowestDeciblesBeforeMute + (-lowestDeciblesBeforeMute / 5 * volume / 20);

        // Completamente muteado a 0 
        if (volume == 0)
        {
            adjustedVolume = -100;
        }

        switch (channel)
        {
            case AudioChannel.General:
                aMixer.SetFloat("GeneralVolume", adjustedVolume);
                //Guardamos variable en playerprefs
                PlayerPrefs.SetInt("GeneralVolume", volume);
                break;

            case AudioChannel.Sound:
                aMixer.SetFloat("SoundVolume", adjustedVolume);
                //Guardamos variable en playerprefs
                PlayerPrefs.SetInt("SoundVolume", volume);
                break;

            case AudioChannel.Music:
                aMixer.SetFloat("MusicVolume", adjustedVolume);
                //Guardamos variable en playerprefs
                PlayerPrefs.SetInt("MusicVolume", volume);
                break;
        }
    }
}
