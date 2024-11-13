using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI instance {  get; private set; }

    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        // check if first time to close the continue button
        newGameButton.onClick.AddListener(() =>
        {

        });
        continueButton.onClick.AddListener(() =>
        {

        });
        settingButton.onClick.AddListener(() =>
        {
            SettingMenuUI.Instance.Show(null);
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

    }
  
} 
