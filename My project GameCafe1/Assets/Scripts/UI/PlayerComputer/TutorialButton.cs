using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Button button;
    private void Start()
    {
        title.text = TutorialMenuUI.Instance.info.infos[transform.GetSiblingIndex()].title;
        button.onClick.AddListener(() =>
        {
            TutorialMenuUI.Instance.modalWindowManager.titleText = title.text;
            TutorialMenuUI.Instance.modalWindowManager.descriptionText = TutorialMenuUI.Instance.info.infos[transform.GetSiblingIndex()].info;
            TutorialMenuUI.Instance.modalWindowManager.Open();
        });

    }
}
