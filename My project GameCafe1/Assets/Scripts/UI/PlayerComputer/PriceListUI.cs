using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
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
        List<CafeManager.AreaItems> AreaItemsList = CafeManager.instance.GetAreaItemsList();
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].value = AreaItemsList[i].area.pricePercentageMultiplicand;
            string Sign;
            if (Mathf.Floor(sliders[i].value) < 0)
            {
                Sign = "-";
            }else if(Mathf.Floor(sliders[i].value) > 0)
            {
                Sign = "+";
            }
            else
            {
                Sign = "";
            }
            slidersText[i].text = $"{Sign}{Mathf.Floor(sliders[i].value)}%";

            int index = i;
            sliders[i].onValueChanged.AddListener((float value) =>
            {
                string Sign;
                if (Mathf.Floor(sliders[i].value) < 0)
                {
                    Sign = "-";
                }
                else if (Mathf.Floor(sliders[i].value) > 0)
                {
                    Sign = "+";
                }
                else
                {
                    Sign = "";
                }
                slidersText[index].text = $"{Sign}{Mathf.Floor(value)}%";
                
                AreaItemsList[index].area.pricePercentageMultiplicand = value;
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
