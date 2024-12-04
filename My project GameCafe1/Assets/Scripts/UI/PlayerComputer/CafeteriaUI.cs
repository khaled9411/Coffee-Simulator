using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CafeteriaUI : MonoBehaviour
{
    public static CafeteriaUI Instance { get; private set; }

    [SerializeField] private Button closeButton;

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
