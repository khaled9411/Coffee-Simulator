using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerSatisfaction : MonoBehaviour
{
    [Range(0, 1)]
    public float satisfactionLevel;
    public Image satisfactionEmoji;
    public Sprite[] emojiSprites;
    private Camera mainCamera;

    void Start()
    {
        satisfactionLevel= Random.Range(0f, 1f);
        mainCamera = Camera.main;
        UpdateSatisfactionEmoji();
    }

    void Update()
    {
        UpdateSatisfactionEmoji();
        //CalculateOverallSatisfaction();
        FaceCamera();
    }

    void FaceCamera()
    {
        if (satisfactionEmoji != null && mainCamera != null)
        {
            satisfactionEmoji.transform.LookAt(mainCamera.transform);
            satisfactionEmoji.transform.rotation = Quaternion.Euler(0f, satisfactionEmoji.transform.rotation.eulerAngles.y, 0f);
        }
    }

    public void UpdateSatisfactionLevel(float changeAmount)
    {
        satisfactionLevel = Mathf.Clamp01(satisfactionLevel + changeAmount);
    }

    void UpdateSatisfactionEmoji()
    {
        if (satisfactionLevel >= 0.8f)
        {
            satisfactionEmoji.sprite = emojiSprites[4];
        }
        else if (satisfactionLevel >= 0.6f)
        {
            satisfactionEmoji.sprite = emojiSprites[3];
        }
        else if (satisfactionLevel >= 0.4f)
        {
            satisfactionEmoji.sprite = emojiSprites[2];
        }
        else if (satisfactionLevel >= 0.2f)
        {
            satisfactionEmoji.sprite = emojiSprites[1];
        }
        else
        {
            satisfactionEmoji.sprite = emojiSprites[0];
        }
    }

    public void CalculateOverallSatisfaction()
    {
        float temperaturePercentage = CafeManager.instance.GetOverallTemperaturePercentage();
        float trashPercentage = CafeManager.instance.GetOverallTrashPercentage();

        float averagePercentage = (temperaturePercentage + trashPercentage) / 2f;

        if (temperaturePercentage >= 0.9f && trashPercentage >= 0.9f)
        {
            UpdateSatisfactionLevel(Random.Range(0.8f, 1f));
        }
        else if (temperaturePercentage >= 0.8f && trashPercentage >= 0.8f)
        {
            UpdateSatisfactionLevel(Random.Range(0.7f, 0.9f));
        }
        else if (temperaturePercentage >= 0.7f && trashPercentage >= 0.7f)
        {
            UpdateSatisfactionLevel(Random.Range(0.6f, 0.8f));
        }
        else if (temperaturePercentage >= 0.6f && trashPercentage >= 0.6f)
        {
            UpdateSatisfactionLevel(Random.Range(0.5f, 0.7f));
        }
        else if (temperaturePercentage >= 0.5f && trashPercentage >= 0.5f)
        {
            UpdateSatisfactionLevel(Random.Range(0.4f, 0.6f));
        }
        else if (averagePercentage < 0.5f)
        {
            UpdateSatisfactionLevel(-Random.Range(0.1f, 0.5f));
        }
    }


}
