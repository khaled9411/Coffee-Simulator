using HUDIndicator;
using Michsky.MUIP;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TutorialSystem : MonoBehaviour
{

    public static TutorialSystem instance;

    [Serializable]
    public struct TutorialPart
    {
        public GameObject Popup;

        public IndicatorOffScreen indicatorOffScreen;
        public IndicatorOnScreen indicatorOnScreen;
    }

    public TutorialPart[] parts;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (var part in parts)
        {
            part.indicatorOnScreen.visible = false;
            part.indicatorOffScreen.visible = false;
        }
    }

    public void OpenTutorialPopupByIndex(int index)
    {
        parts[index].Popup.SetActive(true);
    }

    public void RunTutorialByIndex(int index)
    {
        parts[index].indicatorOnScreen.visible = true;
        parts[index].indicatorOffScreen.visible = true;
    }
}
