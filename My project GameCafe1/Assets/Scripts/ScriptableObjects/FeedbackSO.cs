using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FeedbackSO : ScriptableObject
{
    [Header("General Feedback")]
    public string[] goodFeedback = new string[]
    {
        "Good place!",
        "The arcade machines are amazing.",
        "The price is great for the service!",
        "The computers are really fast.",
        "The staff is very friendly."
    };

    public string[] normalFeedback = new string[]
    {
        "The place is okay.",
        "It’s decent for its price.",
        "Nothing special, but it works.",
        "I enjoyed my time here."
    };

    public string[] badFeedback = new string[]
    {
        "The place needs more improvement.",
        "The machines lag sometimes.",
        "The service could be better.",
        "Not worth the price."
    };

    [Header("Cleanliness Feedback")]
    public string[] placeCleanFeedback = new string[]
    {
        "The place is spotless.",
        "Everything looks tidy and organized.",
        "I appreciate how clean it is here."
    };

    public string[] placeNotCleanFeedback = new string[]
    {
        "The place could use some cleaning.",
        "There’s too much mess around.",
        "Tables and chairs need to be cleaned more often."
    };

    [Header("Temperature Feedback")]
    public string[] temperatureColdFeedback = new string[]
    {
        "It’s too cold in here.",
        "The AC is working too well.",
        "Can you adjust the temperature? It’s freezing."
    };

    public string[] temperatureHotFeedback = new string[]
    {
        "It’s too hot inside.",
        "The AC isn’t working properly.",
        "Feels uncomfortable because of the heat."
    };
}
