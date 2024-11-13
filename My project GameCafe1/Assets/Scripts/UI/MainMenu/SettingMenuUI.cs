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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (isScenePanel) 
        {
            sensitivityGameObject.SetActive(true);
            sensitivitySlider.onValueChanged.AddListener((float value) =>
            {
                 
            });
        }
        else
        {
            sensitivityGameObject.SetActive(false);
        }
        Hide();
        closeButton.onClick.AddListener(Hide);
        musicVolumeSlider.onValueChanged.AddListener((float value) =>
        {
            MusicManager.instance.SetVolume(value);
        });
        soundVolumeSlider.onValueChanged.AddListener((float value) =>
        {
            SoundManager.Instance.SetVolume(value);
        });
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
