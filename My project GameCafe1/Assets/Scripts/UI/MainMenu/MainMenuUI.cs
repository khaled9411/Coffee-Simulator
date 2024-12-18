using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI instance { get; private set; }

    [SerializeField] private Button newGameButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        instance = this;
        CheckSaveFileAndUpdateButtons();
    }

    private void Start()
    {
        newGameButton.onClick.AddListener(StartNewGame);
        continueButton.onClick.AddListener(ContinueGame);
        settingButton.onClick.AddListener(() =>
        {
            SettingMenuUI.Instance.Show(null);
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    private void CheckSaveFileAndUpdateButtons()
    {
        string saveFilePath = Application.persistentDataPath + "/savefile.json";

        bool hasSaveFile = File.Exists(saveFilePath) && new FileInfo(saveFilePath).Length > 0;

        continueButton.interactable = hasSaveFile;
        continueButton.gameObject.SetActive(hasSaveFile);
    }

    private void StartNewGame()
    {
        SaveSystem.ClearData();

        //CheckSaveFileAndUpdateButtons();

        SceneManager.LoadScene("Loading");
    }

    private void ContinueGame()
    {
        SceneManager.LoadScene("Loading");
    }
}