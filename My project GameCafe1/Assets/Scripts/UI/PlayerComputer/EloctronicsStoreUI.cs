using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EloctronicsStoreUI : MonoBehaviour 
{
    public static EloctronicsStoreUI Instance {  get; private set; }

    [SerializeField] private Button closeButton;
    [SerializeField] private GameObject messagePanel;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
        Hide();
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
