using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PriceListUI : MonoBehaviour
{
    public static PriceListUI Instance {  get; private set; }

    [SerializeField] private Button closeButton;
    [SerializeField] private Slider[] sliders;
    [SerializeField] private TextMeshProUGUI[] slidersText;

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
        for(int i = 0; i < sliders.Length; i++)
        {
            int index = i;
            sliders[i].onValueChanged.AddListener((float value) =>
            {
                slidersText[index].text = $"{Mathf.Floor(value)}%";
                // change the price here
            });
        }


        Hide();
    }

    // Update is called once per frame
    void Update()
    {

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
