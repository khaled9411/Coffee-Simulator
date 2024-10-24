using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractHandler : MonoBehaviour
{
    private PlayerRayCaster rayCaster;
    private Targetable target;
    private Targetable previuseTarget;
    private IInteractable interactable;
    private bool isTrigged;

    void Start()
    {
        InputManager.Instance.OnInteract += InputManager_OnInteract;
        rayCaster = GetComponent<PlayerRayCaster>();

        PlayerCollision playerCollision = GetComponent<PlayerCollision>();
        playerCollision.OnPlayerTriggerWithInteractZone += PlayerCollision_OnPlayerTriggerWithInteractZone;
        playerCollision.OnplayerTriggerExitFromInteractZone += PlayerCollision_OnplayerTriggerExitFromInteractZone;
    }

    private void PlayerCollision_OnplayerTriggerExitFromInteractZone()
    {
        interactable = null;
        isTrigged = false;
    }

    private void PlayerCollision_OnPlayerTriggerWithInteractZone(IInteractable obj,string name)
    {
        interactable = obj;
        isTrigged = true;
        ShowTargetName(name);
        ShowInteractButton(interactable.verbName);
        // still did't make the BayableInteractionZone
        if(interactable is Ibuyable)
        {
            ShowPrice((interactable as Ibuyable).GetPrice());
        }
    }

    private void InputManager_OnInteract()
    {
        Interact();
    }
    
    // Update is called once per frame
    void Update()
    {
        //if he is in interaction zone
        if (isTrigged) return;

        target = rayCaster.FireRayCast();
        if (previuseTarget == target) return;

        if (target != null)
        {
            previuseTarget = target;

            ShowTargetName(target.GetName());
            if (target.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                this.interactable = interactable;
                ShowInteractButton(interactable.verbName);
            }
            if(target.TryGetComponent<Ibuyable>(out Ibuyable bayable))
            {
                ShowPrice(bayable.GetPrice());
            }
        }
        else
        {
            previuseTarget = target;
            interactable = null;
            HideAllUIElements();
        }
    }
    private void Interact()
    {
       if(interactable != null)
        {
            interactable.Interact();
            if (isTrigged) PlayerCollision_OnplayerTriggerExitFromInteractZone();
        }
    }
    private void ShowTargetName(string name)
    {
        Debug.Log(name);
    }
    private void ShowInteractButton(string verbName)
    {
        Debug.Log(verbName);
    }
    private void ShowPrice(float price)
    {
        Debug.Log(price);
    }
    private void HideAllUIElements()
    {

    }
}
