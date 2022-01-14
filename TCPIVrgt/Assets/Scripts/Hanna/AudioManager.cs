using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Slider _volumeSlider;

    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            Load();
        }
        else
        {
            Load();
        }
    }


    public void ChangeVolume()
    {
        AudioListener.volume = _volumeSlider.value;
        Save();
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", _volumeSlider.value);
    }

    private void Load()
    {
        _volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }
}
