using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public InteractHandler interactHandler { get; private set; }

    public PlayerMovement playerMovement { get; private set; }

    [SerializeField] private Transform spwanPoint;
    [SerializeField] private Transform PickUpPoint;
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

    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }

    public void SetPickUpPos(Transform pickedItem)
    {
        pickedItem.SetParent(PickUpPoint);
        pickedItem.localPosition = Vector3.zero;
        pickedItem.rotation = Quaternion.identity;
    }
}
