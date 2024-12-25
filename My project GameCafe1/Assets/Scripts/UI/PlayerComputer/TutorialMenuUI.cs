using Michsky.MUIP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMenuUI : MonoBehaviour
{
    public static TutorialMenuUI Instance { get; private set; }

    [SerializeField] private Button closeButton;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private GameObject buttonPrefab;
    public ModalWindowManager modalWindowManager;
    public TutorialsSO info;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        for(int i = 0; i < info.infos.Length; i++) {
            Instantiate(buttonPrefab, buttonParent);
        }

        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
        Hide();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
