using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewVisual : MonoBehaviour
{
    [SerializeField] private GameObject showableGameObject;
    [SerializeField] private GameObject[] visualGameObjectArray;
    private IShowable showable;


    private void Start()
    {
        showable = showableGameObject.GetComponent<IShowable>();
        showable.OnShowPreview += Showable_OnShowPreview;
        showable.OnHidePreview += Showable_OnHidePreview;
        Hide();
    }

    private void Showable_OnHidePreview()
    {
        Hide();
    }

    private void Showable_OnShowPreview()
    {
        Show();
    }
    private void Show()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(true);
        }
    }
    private void Hide()
    {
        foreach (GameObject visualGameObject in visualGameObjectArray)
        {
            visualGameObject.SetActive(false);
        }
    }
}
