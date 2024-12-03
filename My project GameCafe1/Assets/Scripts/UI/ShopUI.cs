using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private Button[] areasButtons;
    [SerializeField] private GameObject itembuttonPrefab;
    [SerializeField] private GameObject itemsButtonnParent;

    
    void Start()
    {
        for(int i = 0; i < areasButtons.Length; i++)
        {
            areasButtons[i].onClick.AddListener(()=>{ 
                for(int j = 0; j< itemsButtonnParent.transform.childCount; j++)
                {
                    Destroy(itemsButtonnParent.transform.GetChild(j).gameObject);
                }
                //for(int j = 0; j< CafeManager.instance) instanite button to every item and fill the data
            });
        }
    }

    
    void Update()
    {
        
    }
}
