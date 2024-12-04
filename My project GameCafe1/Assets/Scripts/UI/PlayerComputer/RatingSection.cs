using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RatingSection : MonoBehaviour
{
    [SerializeField] private Sprite closedStarSprite;
    [SerializeField] private Color closedStarColor;
    [SerializeField] private Sprite starSprite;
    [SerializeField] private Color starColor;

    [SerializeField] private Image[] starsImage;

    private void Start()
    {
        //close all star at the start
        for (int i = 0; i < starsImage.Length; i++)
        {
            CloseStar(i);
        }

        // open the stars based on current area number
        for (int i = 0;i<= CafeManager.instance.GetCurrentAreaIndex(); i++)
        {
            OpenStar(i);
        }
    }
    private void CloseStar(int starIndex)
    {
        starsImage[starIndex].color = closedStarColor;
        starsImage[starIndex].sprite = closedStarSprite;
    }
    private void OpenStar(int starIndex)
    {
        starsImage[starIndex].color = starColor;
        starsImage[starIndex].sprite = starSprite;
    }
}
