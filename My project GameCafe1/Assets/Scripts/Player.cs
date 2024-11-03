using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public InteractHandler interactHandler { get; private set; }

    public PlayerMovement playerMovement { get; private set; }

    [SerializeField] private Transform spwanPoint;
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
        playerMovement = GetComponent<PlayerMovement>();
    }

    public static bool IsPlayer(GameObject gameObject)
    {
        return gameObject == Instance.gameObject;
    }

    public void SpwanInStartPoint()
    {
        transform.position = spwanPoint.position;
    }
}
