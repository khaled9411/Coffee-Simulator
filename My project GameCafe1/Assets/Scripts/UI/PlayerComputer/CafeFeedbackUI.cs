using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CafeFeedbackUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Transform feedbackParent;
    [SerializeField] private GameObject feedbackPrefab;

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
        FeedbackSystem.Instance.OnMyCaffeFeedbackChange += FeedbackSystem_OnMyCaffeFeedbackChange;

        for (int i = 0; i < feedbackParent.childCount; i++)
        {
            Destroy(feedbackParent.GetChild(i).gameObject);
        }
        Hide();
    }

    private void FeedbackSystem_OnMyCaffeFeedbackChange()
    {
        for(int i = 0; i < feedbackParent.childCount; i++)
        {
            Destroy(feedbackParent.GetChild(i).gameObject);
        }
        for(int i = FeedbackSystem.Instance.GetMyCaffeFeedback().Count-1; i>=0; i--)
        {
            Instantiate(feedbackPrefab, feedbackParent).GetComponentInChildren<TextMeshProUGUI>().text = FeedbackSystem.Instance.GetMyCaffeFeedback()[i];
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
