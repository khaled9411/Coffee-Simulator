using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionZone : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject respondableGameObject;
    protected IRespondable respondable;
    public string verbName { get { return respondable.verbName; } set { respondable.verbName = value; } }

    [SerializeField] private GameObject visualsParent;
    [SerializeField] private float distanceToEnable = 5;
    private float distanceToPlayer;

    private Collider interactionZoneCollider;
    private void Start()
    {
        respondable = respondableGameObject.GetComponent<IRespondable>();
        if ( respondable == null ) Debug.LogError("the refreranced gameObject does not inclde IRespodable interface");
        interactionZoneCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if(respondable is IBuyableRespondable)
        {
            if((respondable as IBuyableRespondable).IsPurchased())
            {
                Destroy(gameObject);
            }
        }

        distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        if(distanceToPlayer <= distanceToEnable)
        {
            EnableCollider();
            ShowVisuals();
        }
        else
        {
            DisableCollider();
            HideVisuals();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsShowable())
        {
            if (Player.IsPlayer(other.gameObject))
            {
                (respondable as IShowable).ShowPreview();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (IsShowable())
        {
            if (Player.IsPlayer(other.gameObject))
            {
                (respondable as IShowable).HidePreview();
            }
        }
    }
    
    private bool IsShowable()
    {
        return respondable is IShowable;
    }
    public virtual void Interact()
    {
        respondable.Respond();
    }

    public string GetName()
    {
        return respondable.respondableName;
    }
    private void EnableCollider()
    {
        interactionZoneCollider.enabled = true;
    }
    private void DisableCollider()
    {
        interactionZoneCollider.enabled = false;
    }
    private void ShowVisuals()
    {
        visualsParent.SetActive(true);
    }
    private void HideVisuals()
    {
        visualsParent.SetActive(false);
    }
}
