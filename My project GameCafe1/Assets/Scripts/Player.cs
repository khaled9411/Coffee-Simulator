using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public InteractHandler interactHandler { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("you have two players");
        }
        interactHandler = GetComponent<InteractHandler>();
    }
    
    public static bool IsPlayer(GameObject gameObject)
    {
        return gameObject == Instance.gameObject;
    }
}
