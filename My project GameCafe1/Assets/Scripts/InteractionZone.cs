using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionZone : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject respondableGameObject;
    private IRespondable respondable;
    public string verbName { get { return respondable.verbName; } set { respondable.verbName = value; } }

    private void Start()
    {
        respondable = respondableGameObject.GetComponent<IRespondable>();
        if ( respondable == null ) Debug.LogError("the refreranced gameObject does not inclde IRespodable interface"); 
    }

    public void Interact()
    {
        respondable.respond();
    }

    public string GetName()
    {
        return respondable.Name;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (IsShowable())
        {
            if (TryGetComponent<PlayerCollision>(out _))
            {
                (respondable as IShowable).ShowPreview();
            }
        }
    }

    private bool IsShowable()
    {
        return respondable is IShowable;
    }
}
