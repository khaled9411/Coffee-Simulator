using System;
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
    

    public event Action<string> OnTargetTarget;
    public event Action<string> OnTargetInteractbale;
    public event Action<float> OnTargetBuyable;
    public event Action OnNoTarget;
    private bool hasBroom;
    void Start()
    {
        InputManager.Instance.OnInteract += InputManager_OnInteract;
        InputManager.Instance.OnInteractWithPicked += InputManager_OnInteractWithPicked; ;
        rayCaster = GetComponent<PlayerRayCaster>();

        PlayerCollision playerCollision = GetComponent<PlayerCollision>();
        playerCollision.OnPlayerTriggerWithInteractZone += PlayerCollision_OnPlayerTriggerWithInteractZone;
        playerCollision.OnplayerTriggerExitFromInteractZone += PlayerCollision_OnplayerTriggerExitFromInteractZone;

    }

    private void InputManager_OnInteractWithPicked()
    {
        if(hasBroom)
        {
            hasBroom = false;
        }
    }

    private void PlayerCollision_OnplayerTriggerExitFromInteractZone()
    {
        interactable = null;
        isTrigged = false;
        HideAllUIElements();
    }

    private void PlayerCollision_OnPlayerTriggerWithInteractZone(IInteractable obj,string name)
    {
        interactable = obj;
        isTrigged = true;
        ShowTargetName(name);
        ShowInteractButton(interactable.verbName);
        // still did't make the BayableInteractionZone
        if(IsBuyable())
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

            if (hasBroom && target.TryGetComponent<Trash>(out Trash trash))
            {
                trash.Clean();
            }
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
            if (IsBuyable())
            {
                //Debug.LogError(interactable is BuyableInteractionZone is Area);
                //if (interactable is Area 
                //     && CafeManager.instance.CanOpenNextArea() 
                //     && MoneyManager.Instance.TryBuy((interactable as Ibuyable).GetPrice()))
                //{
                //    interactable.Interact();
                //    if (isTrigged) PlayerCollision_OnplayerTriggerExitFromInteractZone();
                //}

                if (MoneyManager.Instance.TryBuy((interactable as Ibuyable).GetPrice()))
                {
                    
                    interactable.Interact();
                    if (isTrigged) PlayerCollision_OnplayerTriggerExitFromInteractZone();
                }
            }
            else
            {
                interactable.Interact();
            }
        }
    }
    private void ShowTargetName(string name)
    {
        OnTargetTarget?.Invoke(name);
    }
    private void ShowInteractButton(string verbName)
    {
        OnTargetInteractbale?.Invoke(verbName);
    }
    private void ShowPrice(float price)
    {
        OnTargetBuyable?.Invoke(price);
    }
    private void HideAllUIElements()
    {
        Debug.Log("hide");
        OnNoTarget?.Invoke();
    }
    private bool IsBuyable()
    {
        return interactable is Ibuyable;
    }
    public void PickBroom()
    {
        hasBroom = true;
    } 
}
