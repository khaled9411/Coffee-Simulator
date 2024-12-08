using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FeedbackSystem : MonoBehaviour /*, ISaveable*/
{
    public static FeedbackSystem Instance {  get; private set; }
    [SerializeField] private List<string> myCaffeFeedback = new List<string>();
    [SerializeField] private int maxFeedback;
    [SerializeField] private FeedbackSO feedbackSO;

    public event Action OnMyCaffeFeedbackChange;
    //[SerializeField]
    //private string uniqueID;

    //public string UniqueID
    //{
    //    get { return uniqueID; }
    //    private set { uniqueID = value; }
    //}

    //public void LoadData(SaveData data)
    //{
    //    if(data is ListOfStringsSaveData listOfStringsSaveData)
    //    {
    //        myCaffeFeedback = listOfStringsSaveData.value;
    //    }
    //}

    //public SaveData SaveData()
    //{
    //    return new ListOfStringsSaveData(myCaffeFeedback);
    //}

    private void Awake()
    {
        Instance = this; 
    }
    public void HandleFeedback(int tempreature , float satisfaction, int isClean)
    {
        Debug.Log($"goodFeedback is {feedbackSO.goodFeedback.Count()}");
        List<string> availableFeedbacks = new List<string>();
        
        //clean
        if(isClean == -1)
        {
            availableFeedbacks.AddRange(feedbackSO.placeCleanFeedback);
        }else if(isClean == 1) 
        {
            availableFeedbacks.AddRange(feedbackSO.placeNotCleanFeedback);
        }

        //sastifaction
        if (satisfaction >= .6f)
        {
            availableFeedbacks.AddRange(feedbackSO.goodFeedback);
        }
        else if (satisfaction >= .3f)
        {
            availableFeedbacks.AddRange(feedbackSO.normalFeedback);
        }
        else
        {
            availableFeedbacks.AddRange(feedbackSO.badFeedback);
        }

        //tempreature
        if(tempreature == 18)
        {
            availableFeedbacks.AddRange(feedbackSO.temperatureColdFeedback);
        }
        else if(tempreature == 30)
        {
            availableFeedbacks.AddRange(feedbackSO.temperatureHotFeedback);
        }

        PickFeedback(availableFeedbacks);
    }
    private void PickFeedback(List<string> feedbackStrings)
    {
        if(myCaffeFeedback.Count == maxFeedback)
        {
            myCaffeFeedback.RemoveAt(0);
        }
        myCaffeFeedback.Add(feedbackStrings[UnityEngine.Random.Range(0, feedbackStrings.Count)]);
        OnMyCaffeFeedbackChange?.Invoke();
    }
    public FeedbackSO GetFeedbackSO()
    {
        return feedbackSO;
    }
    public List<string> GetMyCaffeFeedback()
    {
        return myCaffeFeedback;
    }
}
