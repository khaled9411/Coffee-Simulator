using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenuUI : MonoBehaviour
{
    public static SettingMenuUI Instance {  get; private set; }

    [SerializeField] private GameObject sensitivityGameObject;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundVolumeSlider;
    [SerializeField] private Button closeButton;
    [SerializeField] private bool isScenePanel;

    private Action onCloseButtonAction;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (isScenePanel) 
        {
            sensitivityGameObject.SetActive(true);
            sensitivitySlider.value = PlayerPrefs.GetFloat(CinemachinePOVExtension.PLAYER_PREFS_SENSITITVITY_MULTIPLAYER, 1f);
            sensitivitySlider.onValueChanged.AddListener((float value) =>
            {
                CinemachinePOVExtension.Instance.SetsensitivityMultiplyer(value);
            });
        }
        else
        {
            sensitivityGameObject.SetActive(false);
        }

        Hide();

        closeButton.onClick.AddListener(()=> {
            if(onCloseButtonAction != null) {
                onCloseButtonAction();
            }
            Hide();
        });

        musicVolumeSlider.value = PlayerPrefs.GetFloat(MusicManager.MUSIC_VOLUME_PLAYER_PREFS, 1f);
        soundVolumeSlider.value = PlayerPrefs.GetFloat(SoundManager.PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);

        musicVolumeSlider.onValueChanged.AddListener((float value) =>
        {
            MusicManager.instance.SetVolume(value);
        });
        soundVolumeSlider.onValueChanged.AddListener((float value) =>
        {
            PlayerPrefs.SetFloat(SoundManager.PLAYER_PREFS_SOUND_EFFECTS_VOLUME, value);
        });
    }
    public void Show(Action onCloseButtonAction)
    {
        this.onCloseButtonAction = onCloseButtonAction;
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
