using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShowable 
{
    // for the preview visuals
    public event Action OnShowPreview;
    public event Action OnHidePreview;
    public void ShowPreview();
    public void HidePreview();
}
