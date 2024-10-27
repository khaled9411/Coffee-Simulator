using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyableInteractionZone : InteractionZone , Ibuyable
{
    public float GetPrice()
    {
        return (respondable as IBuyableRespondable).price;
    }

    public override void Interact()
    {
        base.Interact();
        Destroy(gameObject);
    }
}
