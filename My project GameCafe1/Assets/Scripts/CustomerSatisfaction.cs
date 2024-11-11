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
        mainCamera = Camera.main;
        UpdateSatisfactionEmoji();
    }

    void Update()
    {
        UpdateSatisfactionEmoji();
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

    void UpdateSatisfactionLevel(float changeAmount)
    {
        satisfactionLevel = Mathf.Clamp01(satisfactionLevel + changeAmount);
        UpdateSatisfactionEmoji();
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
}
