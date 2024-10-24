using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Device : MonoBehaviour, IShowable, IBuyableRespondable
{

    [field: SerializeField] public float price { get ; set ; }
    [field: SerializeField] public string respondableName { get ; set; }
    [field: SerializeField] public string verbName { get; set ; }

    [SerializeField] private GameObject visualsParent;
    private Collider DeviceCollider;
    // for the preview visuals
    public event Action OnShowPreview;
    public event Action OnHidePreview;

    private void Start()
    {
        DeviceCollider = GetComponent<Collider>();
        DisableCollider();
        HideVisuals();
        HidePreview();
    }

    public void respond()
    {
        Debug.Log($"you bought a new {respondableName} with {price}$ ");
        EnableCollider();
        ShowVisuals();
        HidePreview();
    }
    private void ShowVisuals()
    {
        if( visualsParent != null )
        {
            visualsParent.SetActive(true);
        }
        else
        {
            Debug.LogWarning("there is no visual parent");
        }
    }
    private void HideVisuals()
    {
        if (visualsParent != null)
        {
            visualsParent.SetActive(false);
        }
        else
        {
            Debug.LogWarning("there is no visual parent");
        }
    }
    public void ShowPreview()
    {
        OnShowPreview?.Invoke();
    }
    public void HidePreview()
    {
        OnHidePreview?.Invoke();
    }
    private void EnableCollider()
    {
        DeviceCollider.enabled = true;
    }
    private void DisableCollider()
    {
        DeviceCollider.enabled = false;
    }
}
