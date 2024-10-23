using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    
    public event Action<IInteractable,string> OnPlayerTriggerWithInteractZone;
    public event Action OnplayerTriggerExitFromInteractZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<InteractionZone>(out InteractionZone interactionZone))
        {
            OnPlayerTriggerWithInteractZone?.Invoke(interactionZone, interactionZone.GetName());
            Debug.Log("you are in zone");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<InteractionZone>(out InteractionZone interactionZone))
        {
            OnplayerTriggerExitFromInteractZone?.Invoke();
            Debug.Log("you are out");
        }
    }
}
