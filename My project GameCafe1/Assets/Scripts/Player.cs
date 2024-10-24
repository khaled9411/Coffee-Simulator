using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  public static Player Instance;

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
    }
    
    public static bool IsPlayer(GameObject gameObject)
    {
        return gameObject == Instance.gameObject;
    }
}
